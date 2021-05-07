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

--[[format]]--
function PrintHelpFormat(actionName, v1Type, v2Type, tip)
    print(string.format("%-9s %-9s %-9s %s", actionName, v1Type, v2Type, tip))
end

--[[init]]--
local todos = LoadConfig()

local action = arg[1]
local v1 = arg[2]
local v2 = arg[3]

--[[action operate]]--
function Add()
    table.insert(todos, NewTodo(v1))
end

function Inster()
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    table.insert(todos, v1, v2)
end

function Del()
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    table.remove(todos, v1)
end

function Swap()
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    v2 = AssertToNumberClimp(v2, 1, #todos, "error")
    local t = todos[v1]
    todos[v1] = todos[v2]
    todos[v2] = t
end

function Done()
    v1 = AssertToNumberClimp(v1, 1, #todos, "error")
    table.remove(todos, v1)
end

function Help()
    PrintHelpFormat("action", "arg1Type", "arg2Type", "tip")
    PrintHelpFormat("a[dd]", "string", "", "Add to-do")
    PrintHelpFormat("i[nster]", "number", "string", "Insert a to-do list to a certain position")
    PrintHelpFormat("d[el]", "number", "", "Delete to-do")
    PrintHelpFormat("s[wap]", "number", "number", "Swap the order of to-dos")
    PrintHelpFormat("done", "number", "", "Complete to-do")
    PrintHelpFormat("h[elp]", "", "", "Help page")
end

local operates = {
    add = { execute = Add, use = false},
    inster = { execute = Inster, use = false},
    del = { execute = Del, use = false},
    swap = { execute = Swap, use = false},
    done = { execute = Done, use = false},
    help = { execute = Help, use = false},
}

local abb2intact = {
    a = "add",
    i = "inster",
    d = "del",
    s = "swap",
    h = "help",
}

--[[run operate]]--
action = abb2intact[action] or action
local operate = operates[action]
if operate then
    operate.execute()
    operate.use = true
end

--[[show todo list]]--
if not operates.help.use then
    for i, v in ipairs(todos) do
        print(i, v.name)
    end
end

SaveConfig(todos)