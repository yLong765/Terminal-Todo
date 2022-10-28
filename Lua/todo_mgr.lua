local todo_mgr = {}
local todos = {}
local file_mgr = require("file_mgr")
local todo_state = {
    wait = "wait",
    work = "work",
    done = "done"
}

function todo_mgr.init()
    todos = {}
    local lines = file_mgr.load_conf()
    for i, line in ipairs(lines) do
        todo_mgr.add(string.unpack("sddds", line))
    end
end

function todo_mgr.create(name, endTime, process, parent, state)
    local todo = {
        name = name or "",
        endTime = endTime or os.time(),
        process = process or 0,
        parent = parent or -1,
        state = state or todo_state.wait,
        childs = {}
    }
    todo.pack = function()
        local pack = string.pack("sddds", todo.name, todo.endTime, todo.process, todo.parent, todo.state) .. "\n"
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
    todo.get_process = function()
        if todo.state == todo_state.done then
            return "100%"
        end
        if #todo.childs == 0 then
            return "0%"
        end
        local count = 0
        local done_count = 0
        for i, child in ipairs(todo.childs) do
            count = count + 1
            if (child.state == todo_state.done) then
                done_count = done_count + 1
            end
        end
        return string.format("%d%%", done_count / count * 100)
    end
    todo.to_string = function(l)
        return string.format("%-" .. l .. "s%-12s%s", todo.name, todo.get_process(), todo.state)
    end
    return todo
end

function todo_mgr.add(name, endTime, process, parent, state)
    if parent == nil or parent == -1 then
        table.insert(todos, todo_mgr.create(name, endTime, process, -1, state))
    else
        todos[parent].add_child(name, endTime, process, parent, state)
    end
end

function todo_mgr.insert(id, name, endTime, process, parent)
    table.insert(todos, todo_mgr.create(name, endTime, process, parent), id)
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
    end
end

function todo_mgr.swap(id1, id2)
    local t = todos[id1]
    todos[id1] = todos[id2]
    todos[id2] = t
end

function todo_mgr.get_todos()
    return todos
end

function todo_mgr.get_childs(id)
    return todos[id].childs
end

function todo_mgr.show()
    if #todos > 0 then
        local maxL = 0
        for id, todo in ipairs(todos) do
            maxL = math.max(maxL, #todo.name)
            for cid, child in ipairs(todo.childs) do
                maxL = math.max(maxL, #child.name)
            end
        end
        print(string.format("%-10s%-".. maxL+5 .."s%-12s%s", "index", "name", "process", "state"))
        for id, todo in ipairs(todos) do
            print(string.format("%-10s%s", id, todo.to_string(maxL + 5)))
            for cid, child in ipairs(todo.childs) do
                print(string.format("%-10s%s", id .. "-" .. cid, child.to_string(maxL + 5)))
            end
        end
    else
        print("No to-do")
    end
end

return todo_mgr
