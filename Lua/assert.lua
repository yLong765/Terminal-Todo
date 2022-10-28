local assert = {}

function assert.number_climp(value, left, right, error_message)
    value = tonumber(value)
    if value == nil or value < left or value > right then
        print(error_message)
        --os.exit()
        return false, value
    end
    return true, value
end

function assert.is_number(value)
    return tonumber(value) ~= nil
end

return assert