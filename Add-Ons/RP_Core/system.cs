// --------------------------
// RP Core :: System
// --------------------------

function deadBrick(%brick)
{
	if(!isObject(%brick))
	{
		//there is no brick
		return true;
	}
	else
	{
		//there is brick
		return false;
	}
}
// System for packages

// Register a package
function RPRegPackage(%pack)
{
	if (%pack $= "")
		return;
	if (!isPackage(%pack))
		return;
	// If the RP is active, then activate the package
	if ($RP::activated)
		activatePackage(%pack);
	if (checkInList($RP::list::packages, %pack))
		return;
	$RP::list::packages = addToList($RP::list::packages, %pack);
}

// Activate packages
function RPActivatePackages()
{
	%packages = $RP::list::packages;
	%words = getWordCount(%packages);
	for (%i = 0; %i < %words; %i++)
	{
		%pack = getWord(%packages, %i);
		// Activate package
		if (isPackage(%pack))
			activatePackage(%pack);
	}
}

// Deactivate packages
function RPDeactivatePackages()
{
	%packages = $RP::list::packages;
	%words = getWordCount(%packages);
	for (%i = 0; %i < %words; %i++)
	{
		%pack = getWord(%packages, %i);
		// Activate package
		if (isPackage(%pack))
			deactivatePackage(%pack);
	}
}

// List functions

// Add to list
function addToList(%list, %data)
{
	if (checkInList(%list, %data))
		return %list;
	
	return (%list $= "") ? %data : %list SPC %data;
}

// Check in list
function checkInList(%list, %data)
{
	return strReplace(%list, %data, "") !$= %list;
}

// Remove from list
function removeFromList(%list, %data)
{
	%list = (getWord(%list, 0) == %data) ? strReplace(%list, %data @ ((getWordCount(%list) == 1) ? "" : " "), "") : strReplace(%list, " " @ %data, "");
	// Clean up
	return trim(strReplace(%list, "  ", " "));
}

// Package

if (isPackage(RP_Core_System))
	deactivatePackage(RP_Core_System);

package RP_Core_System
{
	// When closing server
	function onExit()
	{
		ShutRP();
		
		Parent::onExit();
	}
	
	// When changing map
	function changeMap(%path)
	{
		ShutRP();
		Parent::changeMap(%path);
		RPDeactivatePackages();
	}
	
	// When closing server
	function disconnect(%a)
	{
		ShutRP();
		RPDeactivatePackages();
		
		return Parent::disconnect(%a);
	}
};
RPRegPackage(RP_Core_System);

// Add a mod to the system
function RPRegMod(%mod)
{
	if (%mod $= "")
		return;
	$RP::Mod__[strreplace(%mod, " ", "_")] = 1;
}

// Controlling if a mod exists
function RPModExist(%mod)
{
	if (%mod $= "")
		return 1;
	return $RP::Mod__[strreplace(%mod, " ", "_")];
}

// Find a mod in a list
function RPModsExists(%mods)
{
	if (strlen(%mods) == 0)
		return 1;
	
	%words = getWordCount(%mods);
	for (%i = 0; %i < %words; %i++)
		if (!RPModExist(getWord(%mods, %i)))
			return 0;
	return 1;
}

// Loading functions

// Load file and display for client
function DisplayFile(%client, %file)
{
	if (!GameReadFile(%file))
	{
		messageClient(%client, '', "\c6Unable to find information file.");
		return false;
	}
	
	for (%i = 0; %i < $File::lines; %i++)
		eval("messageClient(" @ %client @ ", '', \"" @ $File::line[%i] @ "\");");
	
	deleteVariables("$File::*");
	
	return true;
}

// Writes to a file ingame
function AppendToFile(%file, %text)
{
	%path = "config/server/RP_Core/" @ %file @ ".txt";
	
	%stream = new fileObject();
	%stream.openForAppend(%path);
	
	%stream.writeLine(%text);
	
	%stream.close();
	%stream.delete();
	
	return true;
}

// Load file and read it
function GamePreloadFile(%file)
{
	%file = findFirstFile(%file);
	if (!isFile(%file))
		return false;
	
	deleteVariables("$Pre::*");
	$Pre::amount = 0;
	
	%stream = new fileObject();
	%stream.openForRead(%file);
	
	while (!%stream.isEOF())
	{
		%line = %stream.readLine();
		if (%line $= "")
			continue;
		
		// Version line
		if (getSubStr(%line, 0, 1) $= "v" && atoi(getSubStr(%line, 1, strlen(%line) - 1) > 0))
			continue;
		// Comment line
		if (getSubStr(%line, 0, 2) $= "//")
			continue;
		
		if (getSubStr(%line, 0, 1) $= " ")
			eval("$Pre::" @ getWord(%line, 1) @ "[" @ $Pre::amount @ "] = \"" @ getWords(%line, 2, getWordCount(%line) - 1) @ "\";");
		else
			eval("$Pre::" @ getWord(%line, 0) @ "[" @ $Pre::amount++ @ "] = \"" @ getWord(%line, 1) @ "\";");
	}
	
	%stream.close();
	%stream.delete();
	
	return true;
}

// Loading a file
function GameReadFile(%file)
{
	%file = findFirstFile(%file);
	if (!isFile(%file))
		return false;
	
	deleteVariables("$File::*");
	
	%stream = new fileObject();
	%stream.openForRead(%file);
	
	for ($File::lines = 0; !%stream.isEOF(); $File::lines++)
		$File::line[$File::lines] = %stream.readLine();
	
	%stream.close();
	%stream.delete();
	
	return true;
}

// Transfers a file to an another file
function GameTransferFile(%source, %target)
{
	%source = findFirstFile(%source);
	if (!isFile(%source))
		return false;
	
	// Open files
	%streamFrom = new fileObject();
	%streamTo = new fileObject();
	%streamFrom.openForRead(%source);
	%streamTo.openForWrite(%target);
	
	// Transferring
	while (!%streamFrom.isEOF())
		%streamTo.writeLine(%streamFrom.readLine());
	
	// Close files
	%streamTo.close();
	%streamFrom.close();
	%streamTo.delete();
	%streamFrom.delete();
	
	// Control check
	if (!isWriteableFileName(findFirstFile(%target)))
	{
		error("GameTransferFile: \"" @ %target @ "\" was not correctly created");
		return false;
	}
	return true;
}

// Check file version
function CheckFileVersion(%file)
{
	%file = findFirstFile(%file);
	if (!isFile(%file))
		return 0;
	
	// Make it quick
	%stream = new fileObject();
	%stream.openForRead(%file);
	%line = %stream.readLine();
	%stream.close();
	%stream.delete();
	
	%test = getSubStr(%line, 0, 1);
	// Invalid version data, return default
	if (%test !$= "v")
		return 0;
	return atoi(getSubStr(%line, 1, strlen(%line) - 1));
}

// Useful functions

// Finds the vocal
function findVowel(%text)
{
	%a = "an";
	switch$ (strLwr(getSubStr(%text, 0, 1)))
	{
		case "a": return %a;
		case "e": return %a;
		case "i": return %a;
		case "o": return %a;
		case "u": return %a;
		case "y": return %a;
		default: return "a";
	}
}

// Returns what sort of number (date only)
function getNumberExtra(%i)
{
	switch (%i)
	{
		case 1: return "st";
		case 21: return "st";
		case 31: return "st";
		case 2: return "nd";
		case 22: return "nd";
		case 3: return "rd";
		case 23: return "rd";
		default: return "th";
	}
}

// Returns a displayable number with a metric prefix
function getMetricPrefix(%number)
{
	// If you are planning to add more, you should be aware
	// of that am unsigned int only takes at maximum 4,294,967,295, 
	// when a signed takes only half
	%n = mAbs(%number);
	// Billion
	if (%n >= 1000000000)
	{
		%text = mFloatLength(%number / 1000000000, 1) @ "G";
	}
	// Million
	else if (%n >= 1000000)
	{
		%text = mFloatLength(%number / 1000000, 1) @ "M";
	}
	// Thousand
	else if (%n >= 1000)
	{
		%text = mFloatLength(%number / 1000, 1) @ "K";
	}
	// Nothing happens
	else
	{
		%text = %number;
	}
	return %text;
}

// Get RGB from Hex
function HexToRGBA(%value)
{
	if (strlen(%value) < 6)
		return "0 0 0 0";
	eval("%R = 0x" @ getSubStr(%value, 0, 2) @ "/255;");
	eval("%G = 0x" @ getSubStr(%value, 2, 2) @ "/255;");
	eval("%B = 0x" @ getSubStr(%value, 4, 2) @ "/255;");
	
	return %R SPC %G SPC %B SPC "1";
}

// Find a client through a text
function findClientByText(%text)
{
	%target = findClientByName(%text);
	if (!isObject(%target))
		%target = findClientByBL_ID(%number);
	return %target;
}

function YN(%value)
{
	if(%value)
		return "Yes";
	return "No";
}
