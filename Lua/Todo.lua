local savePath = "data.conf"
local packfmt = "sd"

function UnpackTodo(line)
    return NewTodo(string.unpack(packfmt, line))
end

function PackTodo(todos)
    local conf = ""
    for i, todo in ipairs(todos) do
        conf = conf .. todo.Pack() .. '\n'
    end
    return conf
end

function SaveConfig(todos)
    local file = io.open(savePath, "wb")
    if file then
        file:write(PackTodo(todos))
        file:close()
    end
end

function LoadConfig()
    local todos = {}
    local file = io.open(savePath, "rb")
    if file then
        for i = 1, math.huge do
            local line = file:read()
            if line == nil then break end
            table.insert(todos, UnpackTodo(line))
        end
    end
    return todos
end

function NewTodo(name, endTime)
    name = name or ""
    endTime = endTime or os.time()
    return {
        name = name,
        endTime = endTime,
        Pack = function()
            return string.pack(packfmt, name, endTime)
        end,
    }
end

--[[Assert]]--

function AssertToNumberClimp(value, left, right, error_message)
    value = tonumber(value)
    if value == nil or value < left or value > right then
        print(error_message)
        os.exit()
    end
    return value
end

--[[主程序]]--

local todos = LoadConfig()

local action = arg[1]
local v1 = arg[2]
local v2 = arg[3]

-- local action = "swap"
-- local v1 = "1"
-- local v2 = "2"

if action == "add" then
    table.insert(todos, NewTodo(v1))
elseif action == "inster" then
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    table.insert(todos, v1, v2)
elseif action == "del" then
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    table.remove(todos, v1)
elseif action == "swap" then
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    v2 = AssertToNumberClimp(v2, 1, #todos, "error")
    local t = todos[v1]
    todos[v1] = todos[v2]
    todos[v2] = t
elseif action == "done" then
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    table.remove(todos, v1)
elseif action == "help" then
    print("add\tstring")
    print("inster\tnumber\tstring")
    print("del\tnumber")
    print("swap\tnumber\tnumber")
    print("done\tnumber")
end

if action ~= "help" then
    for i, v in ipairs(todos) do
        print(i, v.name)
    end
end

SaveConfig(todos)