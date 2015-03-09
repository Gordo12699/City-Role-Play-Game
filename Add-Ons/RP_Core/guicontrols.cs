// --------------------------
// RP Core :: GUI
// --------------------------

// Excuse the mess, but we're renovating

// Predefine variables

$RP::setting::amount = 0;
$RP::stats::amount = 0;
$RP::setting::lastTime = $sim::time;

////////////
// Common //
////////////

/////////////
// Options //
/////////////

// Workground start

// Changing a variable
// function ChangeRPVariable(%var, %value)
// {
	// // Invalid variable
	// if (%var $= "")
		// return false;
	// // Check for cheats
	// if (strstr(%var, ";") >= 0 || strstr(%value, ";") >= 0)
		// return false;
	// %t = $RP::setting::namei_[%var];
	// // Variable exist
	// if (!%t)
		// return false;
	
	// eval("$RP::pref::" @ %var @ " = \"" @ %value @ "\";");
	
	// $RP::setting::var[%t] = %value;
	// RP_PreparePrefList(%t);
	// //RP_PreparePrefTable();
// }

// Register special variables to RP Gui
function RP_RegPref(%name, %cat, %mod, %var, %default, %vartype, %requiresRestart, %hostOnly, %callback)
{
	// Name
	%name = trim(%name);
	if (%name $= "")
	{
		warn("RP_RegPref: Unable to add pref: Invalid name");
		return false;
	}
	if ($RP::setting::namei_[%name])
	{
		warn("RP_RegPref: Unable to add pref: Name already exist '" @ %name @ "' from '" @ $RP::setting::mod[$RP::setting::namei_[%name]] @ "'");
		return false;
	}
	
	// Category
	%cat = trim(%cat);
	if (%cat $= "")
	{
		warn("RP_RegPref: Unable to add pref: Invalid category");
		return false;
	}
	
	// Mod
	%mod = trim(%mod);
	if (%mod $= "")
	{
		warn("RP_RegPref: Unable to add pref: Invalid mod");
		return false;
	}
	
	// Variable
	%var = trim(%var);
	if (%var $= "" || %var << 0 != 0)
	{
		warn("RP_RegPref: Unable to add pref: Invalid variable '" @ %var @ "'");
		return false;
	}
	if ($RP::setting::var_i[%var])
	{
		warn("RP_RegPref: Unable to add pref: Variable already exist '" @ %var @ "'");
		return false;
	}
	%varName = "$RP::pref::" @ %var;
	
	// Variable type
	%a = getWordCount(%vartype);
	%type = firstWord(%vartype);
	switch$ (%type)
	{
		case "int":
			for (%i = 1; %i <= 3; %i++)
			{
				%var[%i] = getWord(%vartype, %i) << 0;
				if (%var[%i] != mFloor(%var[%i]))
				{
					warn("RP_RegPref: Unable to add pref: Invalid vartype '" @ %var[%i] @ "'");
					return false;
				}
			}
			%varamount = 3;
		case "list":
			if (%a % 2 == 0)
			{
				warn("RP_RegPref: Unable to add pref: Invalid vartype '" @ %vartype @ "'");
				return false;
			}
			%varamount = 0;
			for (%i = 1; %i <= %a-1; %i += 2)
			{
				%var[%i] = getWord(%vartype, %i);
				%var[%i+1] = getWord(%vartype, %i+1);
				%varamount++;
			}
		case "bool":
			%varamount = 0;
		case "string":
			for (%i = 1; %i <= 2; %i++)
			{
				%var[%i] = getWord(%vartype, %i) + 0;
			}
			%varamount = 2;
			%var[1] = (%var[1] > 200) ? 200 : mAbs(%var[1]);
			%var[2] = (%var[2] > 150) ? 150 : mAbs(%var[2]);
		default:
			warn("RP_RegPref: Unable to add pref: Invalid vartype");
			return false;
	}
	
	// Requires restart
	%requiresRestart = %requiresRestart << 0;
	%requiresRestart = (%requiresRestart == 1) ? 1 : 0;
	
	// Host only
	%hostOnly = %hostOnly + 0;
	%hostOnly = (%hostOnly == 1) ? 1 : 0;
	
	// Default
	eval(%varName @ "=\"" @ %default @ "\";");
	
	$RP::setting::amount++;
	%b = $RP::setting::amount;
	
	// Add to var cache
	$RP::setting::namei_[%name] = %b;
	$RP::setting::var_i[%var] = %b;
	
	// Add to list
	$RP::setting::name[%b] = %name;
	$RP::setting::cat[%b] = %cat;
	$RP::setting::mod[%b] = %mod;
	$RP::setting::var[%b] = %var;
	$RP::setting::default[%b] = %default;
	$RP::setting::vartype[%b] = %vartype;
	$RP::setting::requiresRestart[%b] = %requiresRestart;
	$RP::setting::hostOnly[%b] = %hostOnly;
	$RP::setting::callback[%b] = %callback;
	
	// Prepare gui list
	%cmdList = RP_PreparePrefList(%b);
	//%cmdList = %name TAB %mod TAB %var TAB %default TAB %vartype TAB %requiresRestart;
	//$RP::setting::cmdList[%b] = %cmdList;
	
	// Add to variable table
	//$RP::setting::cmdTable = $RP::setting::cmdTable NL %cmdList;
	
	// Send to online users
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (%hostOnly && %client.bl_id != getNumKeyID())
			continue;
		
		if (%client.isSuperAdmin)
			commandToClient(ClientGroup.getObject(%i), 'addRPPrefs', %cmdList);
	}
	
	return true;
}

// Prepares a variable list
function RP_PreparePrefList(%i)
{
	if (%i <= 0 || %i > $RP::setting::amount)
		return "";
	
	return $RP::setting::cmdList[%i] = $RP::setting::name[%i] TAB $RP::setting::cat[%i] TAB $RP::setting::mod[%i] TAB $RP::setting::var[%i] TAB $RP::setting::default[%i] TAB $RP::setting::vartype[%i] TAB $RP::setting::requiresRestart[%i];
}

// Get a table of RP variables
function RP_PreparePrefTable()
{
	if ($RP::setting::amount == 0)
		return;
	
	for (%i = 1; %i <= $RP::setting::amount; %i++)
	{
		%cmdListHost = (%cmdListHost $= "") ? $RP::setting::cmdList[%i] : %cmdListHost NL $RP::setting::cmdList[%i];
		if ($RP::setting::hostOnly[%i])
			continue;
		%cmdList = (%cmdList $= "") ? $RP::setting::cmdList[%i] : %cmdList NL $RP::setting::cmdList[%i];
	}
	//$RP::setting::cmdTableHost = %cmdListHost;
	//$RP::setting::cmdTable = %cmdList;
	
	// Update last time
	$RP::setting::lastTime = $sim::time;
}

// Send RP variables to clients
function RP_SendVariables()
{
	if ($RP::setting::amount == 0)
		return;
	
	//RP_PreparePrefTable();
	
	// Send to online users
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		// Host
		//if (%client.bl_id != getNumKeyID())
		//	commandToClient(%client, 'addRPPrefs', $RP::setting::cmdTableHost);
		//else if (%client.isSuperAdmin)
		//	commandToClient(%client, 'addRPPrefs', $RP::setting::cmdTable);
	}
}

// Workground end

///////////
// Stats //
///////////

// General information
// %name, %var, %info, %varList, %prefix, %suffix, %hide
// %name - The name that will be displayed
// %var - The name of the var from the db
// %info - If it will be displayed in the bottomprint
// %varList - The list name if the var means something. Else a function name
// %prefix - Displayed before the value
// %suffix - Displayed after the value
// %hide - Hide if empty empty value

// Adding stats information
function RP_AddStat(%name, %var, %info, %varList, %prefix, %suffix, %hide)
{
	if (!isObject(RPDB))
		return;
	
	%name = trim(%name);
	if (%name $= "")
		return;
	
	%info = %info + 0;
	%info = (%info == 1) ? 1 : 0;
	%hide = %hide + 0;
	%hide = (%hide == 1) ? 1 : 0;
	
	// Function for the variable
	if (isFunction(%varList))
	{
		%varList = %varList;
	}
	// List for the variable
	else if (%varList !$= "")
	{
		if (getSubStr(%varList, 0, 1) $= "$")
		{
			%varList = getSubStr(%varList, 1, strlen(%varList));
		}
	}
	
	// Exist in DB
	if (RPDB.findValue(%var) == 0)
		return;
	
	// Exist in the system
	if ($RP::stats::namei_[%var])
		return;
	
	$RP::stats::amount++;
	%i = $RP::stats::amount;
	
	$RP::stats::namei_[%name] = %i;
	
	$RP::stats::name[%i] = %name;
	$RP::stats::var[%i] = %var;
	$RP::stats::info[%i] = %info;
	$RP::stats::varList[%i] = %varList;
	$RP::stats::prefix[%i] = %prefix;
	$RP::stats::suffix[%i] = %suffix;
	$RP::stats::hide[%i] = %hide;
	
	%cmdList = RP_PrepareStatList(%i);
}

// Prepares a stats list
function RP_PrepareStatList(%i)
{
	if (%i <= 0 || %i > $RP::stats::amount)
		return "";
	
	return $RP::stats::cmdList[%i] = $RP::stats::name[%i] TAB $RP::stats::info[%i] TAB $RP::stats::prefix[%i] TAB $RP::stats::suffix[%i];
}

// Send RP stats to clients
function RP_SendStatsTable()
{
	if ($RP::stats::amount == 0)
		return;
	
	// Send to online users
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		//commandToClient(ClientGroup.getObject(%i), 'addRPStats', $RP::stats::cmdTable);
	}
}

// Get stat var
function RP_GetStatVar(%i, %ID)
{
	if (%i <= 0 || %i > $RP::stats::amount)
		return "";
	%var = RPDB.get(%ID, $RP::stats::var[%i]);
	%varList = $RP::stats::varList[%i];
	if (%varList !$= "")
	{
		if (isFunction(%varList))
			eval("%var = " @ %varList @ "(\"" @ %var @ "\");");
		else
			eval("%var = $" @ %varList @ "[\"" @ %var @ "\"];");
	}
	return %var;
}

// Get stat var from BL ID
function RP_GetStatVarFromID(%i, %ID)
{
	if (%i <= 0 || %i > $RP::stats::amount)
		return "";
	%var = RPDB.get(%ID, $RP::stats::var[%i]);
	%varList = $RP::stats::varList[%i];
	if (%varList !$= "")
	{
		if (isFunction(%varList))
			eval("%var = " @ %varList @ "(\"" @ %var @ "\");");
		else
			eval("%var = $" @ %varList @ "[\"" @ %var @ "\"];");
	}
	return %var;
}

// Get stat var from RP Data
function RP_GetStatVarFromData2(%i, %data)
{
	if (%i <= 0 || %i > $RP::stats::amount)
		return "";
	if (!isObject(%data))
		return "";
	
	%var = %data.value[$RP::stats::var[%i]];
	%varList = $RP::stats::varList[%i];
	if (%varList !$= "")
	{
		if (isFunction(%varList))
			eval("%var = " @ %varList @ "(\"" @ %var @ "\");");
		else
			eval("%var = $" @ %varList @ "[\"" @ %var @ "\"];");
	}
	return %var;
}

////////////
// Client //
////////////

// Send add-on information to GUI
function GameConnection::SendModToGUI(%client, %mods)
{
	commandToClient(%client, 'ControlRPGUI', %mods);
}

// To have or not to have
function GameConnection::HaveRPGUI(%client, %mod)
{
	return %client.RPGUI__[%mod] || 0;
	//return strlen(%mod) != 0 && strstr(%client.RPGUIMods, strlwr(%mod)) >= 0;
}

// Sends preferences to GUI
function GameConnection::RP_SendPrefs(%client)
{
	if ($RP::setting::amount == 0)
		return;
	
	if (!isObject(%client))
		return;
	
	// Host
	//if (%client.bl_id != getNumKeyID())
	//	commandToClient(%client, 'addRPPrefs', $RP::setting::cmdTableHost);
	//else if (%client.isSuperAdmin)
	//	commandToClient(%client, 'addRPPrefs', $RP::setting::cmdTable);
}

//////////////
// Commands //
//////////////

// Get variables to client
// function ServerCmdGetRPPrefs(%client)
// {
	// if (!%client.isSuperAdmin)
		// return false;
	
	// %client.RP_SendPrefs();
// }

// Change variables
// function ServerCmdChangeRPVariables(%client, %vars, %values)
// {
	// if (!%client.isSuperAdmin)
		// return false;
	
	// // Just for security
	// if (getFieldCount(%vars) == getFieldCount(%values))
		// return false;
	
	// for (%i = 0; %i < getFieldCount(%vars); %i++)
	// {
		// ChangeRPVariable(getField(%vars, %i), getField(%values, %i));
	// }
// }

// Adds the GUIs the client have
function ServerCmdCheckRPGUI(%client, %GUIs)
{
	%n = getWordCount(%GUIs);
	for (%i = 0; %i < %c; %i++)
		%client.RPGUI__[getWord(%GUIs, %i)] = 1;
	//%client.RPGUIMods = %client.RPGUIMods SPC %GUIs;
}
