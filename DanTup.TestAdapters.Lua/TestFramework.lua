dofile(arg[1])

local executeTests = true
for i = 2, #arg do
	if arg[i] == "list" then executeTests = false end
end

local function StringStarts(s, start)
	return s:sub(1, start:len()) == start
end

local function XmlEncode(s)
	-- TODO: Make this better
	return (s or ""):gsub("<", "&lt;"):gsub(">", "&gt;")
end


allTests = {}
for name, value in pairs(_G) do
	if StringStarts(name, "test_") then
		name = name:sub(6)
		allTests[name] = value
	end
end

local function ExecuteTests()
	print("<Tests>")
	failcount = 0
	for name, value in pairs(allTests) do
		if testframework_setup ~= nil then
			testframework_setup()
		end

		local printStack = function(err) return { err, debug.traceback(err) } end
		local pass, err = xpcall(value, printStack)

		local info = debug.getinfo(value)

		print("	<Test>")
		print("		<Name>"..XmlEncode(name).."</Name>")
		print("		<DisplayName>"..XmlEncode(name:gsub("_", " ")).."</DisplayName>")
		print("		<CodeFilePath>"..info.source:sub(2).."</CodeFilePath>")
		print("		<LineNumber>"..info.linedefined.."</LineNumber>")
		print("		<Outcome>"..(pass and "Passed" or "Failed").."</Outcome>")
		if not pass then
			print("		<ErrorMessage>"..XmlEncode(err[1]).."</ErrorMessage>")
			print("		<ErrorStackTrace>"..XmlEncode(err[2]).."</ErrorStackTrace>")
		end
		print("	</Test>")
		if not pass then failcount = failcount + 1 end

		if testframework_teardown ~= nil then
			testframework_teardown()
		end
	end
	print("</Tests>")

	if failcount > 0 then
		os.exit(1)
	end
end

local function ListTests()
	print("<Tests>")
	for name, value in pairs(allTests) do
		local info = debug.getinfo(value)

		print("	<Test>")
		print("		<Name>"..XmlEncode(name).."</Name>")
		print("		<DisplayName>"..XmlEncode(name:gsub("_", " ")).."</DisplayName>")
		print("		<CodeFilePath>"..info.source:sub(2).."</CodeFilePath>")
		print("		<LineNumber>"..info.linedefined.."</LineNumber>")
		print("	</Test>")
	end
	print("</Tests>")
end

if executeTests then
	ExecuteTests()
else
	ListTests()
end
