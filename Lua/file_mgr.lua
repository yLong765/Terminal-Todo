local file_mgr = {}
local save_path = "data.conf"

function file_mgr.pack(todos)
    local conf = ""
    for i, todo in ipairs(todos) do
        conf = conf .. todo.pack()
    end
    return conf
end

function file_mgr.save_conf(todos)
    local file = io.open(save_path, "wb")
    if file then
        file:write(file_mgr.pack(todos))
        file:close()
    end
end

function file_mgr.load_conf()
    local lines = {}
    local file = io.open(save_path, "rb")
    if file then
        for i = 1, math.huge do
            local line = file:read()
            if line == nil then break end
            table.insert(lines, line)
        end
    end
    return lines
end

return file_mgr