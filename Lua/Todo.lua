local todo_mgr = require("todo_mgr")
local file_mgr = require("file_mgr")
local assert = require("assert")

--[[format]]
function PrintHelpFormat(actionName, v1Type, v2Type, tip)
    print(string.format("%-9s| %-9s| %-9s| %s", actionName, v1Type, v2Type, tip))
end

todo_mgr.init()

--[[action operate]]
function Add(v1, v2)
    if v2 ~= nil and assert.is_number(v1) then
        local res, v = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
        if res then
            todo_mgr.add(v2, nil, nil, v)
        end
    else
        todo_mgr.add(v1)
    end
end

function Inster(v1, v2)
    local res, v = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    if res then
        todo_mgr.insert(v, v2)
    end
end

function Del(v1, v2)
    local res1
    local res2 = true
    local nv1, nv2
    res1, nv1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    if assert.is_number(v2) then
        res2, nv2 = assert.number_climp(v2, 1, #todo_mgr.get_childs(v1), "The second number is incorrect")
    end
    if res1 and res2 then
        todo_mgr.del(nv1, nv2)
    end
end

function Swap(v1, v2)
    local res1, nv1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    local res2, nv2 = assert.number_climp(v2, 1, #todo_mgr.get_todos(), "The second number is incorrect")
    if res1 and res2 then
        todo_mgr.swap(nv1, nv2)
    end
end

function Done(v1, v2)
    local res1
    local res2 = true
    local nv1, nv2
    res1, nv1 = assert.number_climp(v1, 1, #todo_mgr.get_todos(), "The first number is incorrect")
    if assert.is_number(v2) then
        res2, nv2 = assert.number_climp(v2, 1, #todo_mgr.get_childs(v1), "The second number is incorrect")
    end
    if res1 and res2 then
        todo_mgr.done(nv1, nv2)
    end
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
    --PrintHelpFormat("show", "string", "string", "")
    PrintHelpFormat("exit", "", "", "exit todo software")
    PrintHelpFormat("h[elp]", "", "", "Help page")
end

local operates = {
    add = {execute = Add},
    inster = {execute = Inster},
    del = {execute = Del},
    swap = {execute = Swap},
    done = {execute = Done},
    help = {execute = Help}
}

local abb2intact = {
    a = "add",
    i = "inster",
    d = "del",
    s = "swap",
    h = "help"
}

--[[run operate]]
function RunOperate(action, v1, v2)
    local opId =abb2intact[action] or action
    local operate = operates[opId]
    if operate then
        operate.execute(v1, v2)
    end
end
--[[show todo list]]
function ShowTodoList(action)
    if abb2intact[action] == nil then
		print("Please use the 'h' or 'help' to view help")
    elseif abb2intact[action] ~= "help" then
		todo_mgr.show()
    end
    print()
end

function Read()
    io.write("> ")
    return ParserString(io.read())
end

function ParserString(str)
    local strList = {}
    string.gsub(str, '[^ ]+', function (w)
        table.insert(strList, w)
    end)
    return strList[1], strList[2], strList[3]
end

--[[main function]]
function Main()
    local action, v1, v2 = Read()
    while action ~= "exit" do
        RunOperate(action, v1, v2)
        ShowTodoList(action)
        file_mgr.save_conf(todo_mgr.get_todos())
        action, v1, v2 = Read()
    end
end

Main()