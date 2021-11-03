local todo_mgr = require("todo_mgr")
local file_mgr = require("file_mgr")
local assert = require("assert")

--[[format]]
function PrintHelpFormat(actionName, v1Type, v2Type, tip)
    print(string.format("%-9s| %-9s| %-9s| %s", actionName, v1Type, v2Type, tip))
end

--[[init]]
local action = arg[1]
local v1 = arg[2]
local v2 = arg[3]

-- local action = "a"
-- local v1 = "1"
-- local v2 = "打卡"

todo_mgr.init()

--[[action operate]]
function Add()
    if v2 ~= nil and assert.is_number(v1) then
        v1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
        todo_mgr.add(v2, nil, nil, v1)
    else
        todo_mgr.add(v1)
    end
end

function Inster()
    v1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    todo_mgr.insert(v1, v2)
end

function Del()
    v1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    if assert.is_number(v2) then
        v2 = assert.number_climp(v2, 1, #todo_mgr.get_childs(v1), "The second number is incorrect")
    end
    todo_mgr.del(v1, v2)
end

function Swap()
    v1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    v2 = assert.number_climp(v2, 1, #todo_mgr.get_todos(), "The second number is incorrect")
    todo_mgr.swap(v1, v2)
end

function Done()
    v1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    if assert.is_number(v2) then
        v2 = assert.number_climp(v2, 1, #todo_mgr.get_childs(v1), "The second number is incorrect")
    end
    todo_mgr.done(v1, v2)
end

function Help()
    PrintHelpFormat("action", "arg1Type", "arg2Type", "tip")
    print("------------------------------------------")
    PrintHelpFormat("a[dd]", "string", "", "Add to-do")
    PrintHelpFormat("a[dd]", "number", "string", "Add child to-do  for [number] to-do")
    PrintHelpFormat("i[nster]", "number", "string", "Insert a to-do list to a certain position")
    PrintHelpFormat("d[el]", "number", "", "Delete to-do")
    PrintHelpFormat("d[el]", "number", "number", "Delete child to-do  for [number] to-do")
    PrintHelpFormat("s[wap]", "number", "number", "Swap the order of to-dos")
    PrintHelpFormat("done", "number", "", "Complete to-do")
    PrintHelpFormat("done", "number", "number", "Complete child to-do  for [number] to-do")
    PrintHelpFormat("h[elp]", "", "", "Help page")
end

local operates = {
    add = {execute = Add, useCount = 0},
    inster = {execute = Inster, useCount = 0},
    del = {execute = Del, useCount = 0},
    swap = {execute = Swap, useCount = 0},
    done = {execute = Done, useCount = 0},
    help = {execute = Help, useCount = 0}
}

local abb2intact = {
    a = "add",
    i = "inster",
    d = "del",
    s = "swap",
    h = "help"
}

--[[run operate]]
action = abb2intact[action] or action
local operate = operates[action]
if operate then
    operate.execute()
    operate.useCount = operate.useCount + 1
end

--[[show todo list]]
if operates.help.useCount == 0 then
    todo_mgr.show()
end

file_mgr.save_conf(todo_mgr.get_todos())
