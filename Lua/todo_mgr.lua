local todo_mgr = {}
local todos = {}
local file_mgr = require("file_mgr")
local todo_state = {
    wait = "wait",
    work = "work",
    done = "done",
}

function todo_mgr.init()
    todos = {}
    local lines = file_mgr.load_conf()
    for i, line in ipairs(lines) do
        todo_mgr.add(string.unpack("sdddss", line))
    end
end

function todo_mgr.create(name, endTime, process, parent, state, tag)
    local todo = {
        name = name or "",
        endTime = endTime or os.time(),
        process = process or 0,
        parent = parent or -1,
        state = state or todo_state.wait,
        tag = tag or "#",
        childs = {}
    }
    todo.pack = function()
        local pack = string.pack("sdddss", todo.name, todo.endTime, todo.process, todo.parent, todo.state, todo.tag) .. "\n"
        for i, child in ipairs(todo.childs) do
            pack = pack .. child.pack()
        end
        return pack
    end
    todo.add_child = function(name, endTime, process, parent, state)
        table.insert(todo.childs, todo_mgr.create(name, endTime, process, parent, state))
    end
    todo.del_child = function(id)
        table.remove(todo.childs, id)
    end
    todo.is_child_all_done = function ()
        for i, child in ipairs(todo.childs) do
            if child.state ~= todo_state.done then
                return false
            end
        end
        return true
    end
    todo.set_tag = function(tag)
        todo.tag = tag
    end
    todo.to_string = function(name_l, tag_l)
        return string.format("%-" .. name_l .. "s%-".. tag_l .."s", todo.name, todo.tag)
    end
    return todo
end

function todo_mgr.add(name, endTime, process, parent, state, tag)
    if parent == nil or parent == -1 then
        table.insert(todos, todo_mgr.create(name, endTime, process, -1, state, tag))
    else
        todos[parent].add_child(name, endTime, process, parent, state, tag)
    end
end

function todo_mgr.set_tag(id, tag)
    todos[id].set_tag(tag)
end

function todo_mgr.insert(id, name, endTime, process, parent)
    table.insert(todos, id, todo_mgr.create(name, endTime, process, parent))
end

function todo_mgr.del(id1, id2)
    if id2 == nil then
        table.remove(todos, id1)
    else
        table.remove(todos[id1].childs, id2)
    end
end

function todo_mgr.done(id1, id2)
    if id2 == nil then
        todos[id1].state = todo_state.done
    else
        todos[id1].childs[id2].state = todo_state.done
        if todos[id1].is_child_all_done() then
            todos[id1].state = todo_state.done
        end
    end
    if todos[id1].state == todo_state.done then
        todo_mgr.sort_state()
    end
end

function todo_mgr.swap(id1, id2)
    local t = todos[id1]
    todos[id1] = todos[id2]
    todos[id2] = t
end

function todo_mgr.sort_tag()
    table.sort(todos, function (l, r)
        return l.tag > r.tag
    end)
end

function todo_mgr.sort_state()
    table.sort(todos, function (l, r)
        if l.state == r.state then
            return l.tag > r.tag
        end
        return l.state > r.state
    end)
end

function todo_mgr.get_todos(tag)
    return todos
end

function todo_mgr.get_childs(id)
    return todos[id].childs
end

function todo_mgr.get_name_max_length()
    local maxL = 4
    for id, todo in ipairs(todos) do
        maxL = math.max(maxL, #todo.name)
        for cid, child in ipairs(todo.childs) do
            maxL = math.max(maxL, #child.name)
        end
    end
    return maxL + 5
end

function todo_mgr.get_tag_max_length()
    local maxL = 3
    for id, todo in ipairs(todos) do
        maxL = math.max(maxL, #todo.tag)
    end
    return maxL + 5
end

function todo_mgr.show(v1)
    local new_todos = {}
    if v1 == nil then
        for i, todo in ipairs(todos) do
            if todo.state ~= todo_state.done then
                new_todos[i] = todo
            end
        end
    elseif v1 == todo_state.done then
        for i, todo in ipairs(todos) do
            if todo.state == todo_state.done then
                new_todos[i] = todo
            end
        end
    else
        for i, todo in ipairs(todos) do
            if todo.tag == v1 and todo.state ~= todo_state.done then
                new_todos[i] = todo
            end
        end
    end
    todo_mgr.show_todos(new_todos)
end

function todo_mgr.show_todos(todos)
    if next(todos) ~= nil then
        local max_name_l = todo_mgr.get_name_max_length()
        local max_tag_l = todo_mgr.get_tag_max_length()
        print(string.format("%-10s%-".. max_name_l .."s%-".. max_tag_l .."s", "Index", "Name", "Tag"))
        for id, todo in pairs(todos) do
            print(string.format("%-10s%s", id, todo.to_string(max_name_l, max_tag_l)))
            for cid, child in ipairs(todo.childs) do
                print(string.format("%-10s%s", id .. "-" .. cid, child.to_string(max_name_l, max_tag_l)))
            end
        end
    else
        print("No to-do")
    end
end

return todo_mgr
