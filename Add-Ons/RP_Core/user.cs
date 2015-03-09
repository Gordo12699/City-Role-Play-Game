// --------------------------
// RP Core :: User
// --------------------------

function removeMoney(%col, %client, %arg1)
{
    if(!%col.isPlanted())
    {
        CityRPGData.getData(%client.bl_id).valueMoney += %arg1;
        messageClient(%client, '', "Your money has been returned to you because you were unable to plant the lot!");
    }
}
	
if (isPackage(RP_Core_User))
	deactivatePackage(RP_Core_User);

package RP_Core_User
{
	// Player entering a game
	function GameConnection::onClientEnterGame(%client)
	{
		Parent::onClientEnterGame(%client);
		
		if (!%client.isInRP())
			%client.EnterRPGame();
	}
	
	// Player leaving the game
	function GameConnection::onClientLeaveGame(%client)
	{
		if (%client.isInRP())
			%client.LeaveRPGame();
		
		Parent::onClientLeaveGame(%client);
	}
	
	// Spawn player
	function GameConnection::spawnPlayer(%client)
	{
		// Angus ghost
		if (isObject(%client.player))
			%client.player.delete();
		Parent::spawnPlayer(%client);
		
		if (%client.isInRP())
			%client.schedule(50, FinishRPSpawning);
		
			
		
	}
	
	// When the players gets killed
	function GameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc)
	{
		Parent::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc);
		
		// Loose tools
		if (isObject(%client) && %client.isInRP())
			%client.RPData.value["tools"] = "";
	}
	
	function GameConnection::setControlObject(%client, %obj)
	{
		Parent::setControlObject(%client, %obj);
		if (%client.player == %obj && %client.isInRP())
			commandToClient(%player.client, 'PlayGui_CreateToolHud', $RP::pref::user::amountTools);
	}
	
	// Change datablock
	function Player::changeDataBlock(%player, %datablock, %client)
	{
		if (isObject(RP_MiniGame) && %client.isInRP() && isObject(%client.RP_PlayerDatablock))
		{
			commandToClient(%player.client, 'PlayGui_CreateToolHud', %client.RP_PlayerDatablock.maxTools);
			// Force player datablock
			Parent::changeDataBlock(%player, %client.RP_PlayerDatablock, %client);
			// And remove it
			%client.RP_PlayerDatablock = "";
		}
		else if (isObject(RP_MiniGame) && %client.isInRP() && isObject($RP::pref::user::playerModel))
		{
			commandToClient(%player.client, 'PlayGui_CreateToolHud', $RP::pref::user::playerModel.maxTools);
			// Force player datablock
			Parent::changeDataBlock(%player, $RP::pref::user::playerModel, %client);
		}
		else
		{
			// Ordinary
			Parent::changeDataBlock(%player, %datablock, %client);
		}
	}
	
	// Evade scaling
	function Player::setScale(%player, %scale)
	{
		%client = %player.client;
		if (%client.isInRP() && !$RP::pref::user::playerScaling)
			%scale = $RP::pref::user::playerScale;
		Parent::setScale(%player, %scale);
	}
	
	// Activate stuffs
	function Player::activateStuff(%player)
	{
		Parent::activateStuff(%player);
		
		// Retrieve target
		%obj = %player.GetObjectFromPOW($TypeMasks::fxBrickObjectType);
		
		if (!isObject(%obj))
			return 0;
		
		%client = %player.client;
		if (!%client.isInRP())
			return 0;
		// Avoid player press too many times
		if (!%client.CheckLastAction(%obj))
			return 0;
		
		// Brick
		if (%obj.getType() & $TypeMasks::fxBrickObjectType)
		{
			%datablock = %obj.getDataBlock();
			
			// Info brick
			if (%datablock.isInfoBrick)
			{
				%mods = %client.RPGUIMods;
				%mod = strlwr(%datablock.mod);
				%cmd = %datablock.guiCmd;
				%name = %datablock.getName();
				// GUI
				if (strlen(%cmd) != 0 && isFunction(%name, "ConstructGUI") && %client.HaveRPGUI(%mod))
				{
					%datablock.ConstructGUI(%client);
					commandToClient(%client, 'DisplayRPGUI', %cmd);
				}
				// Command
				else if (isFunction(%name, "Activate"))
				{
					if (%datablock.Activate(%client, %obj) != false)
					{
						messageClient(%client, '', "\c30. \c6Exit");
						%client.ThrowLastActive();
					}
				}
				// Invalid
				else
				{
					commandToClient(%client, 'centerPrint', "\c2Invalid clickable brick.", 2);
				}
				%client.lastAction = $sim::time;
			}
		}
		return 1;
	}
	
	function Armor::onNewDatablock(%armor, %player)
	{
		Parent::onNewDatablock(%armor, %player);
		%client = %player.client;
		if (!isObject(%client))
			return;
		
		commandToClient(%client, 'PlayGui_CreateToolHud', %armor.maxTools);
		for (%i = 0; %i < %armor.maxTools; %i++)
		{
			%tool = %player.tool[%i];
			messageClient(%client, 'MsgItemPickup', "", %i, (isObject(%tool) ? %tool.getID() : 0), 1);
		}
	}
	
	function KeyProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
	{
		parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
		
		if(%col.getDatablock().getClassName() $= "WheeledVehicleData" && mFloor(VectorLen(%col.getvelocity())) == 0)
		{
			if(getTrustLevel(%col.brickGroup, %obj.client.brickGroup) > 0)
			{
				%col.locked = !%col.locked;
				commandToClient(%obj.client, 'centerPrint', "\c6The vehicle is now \c3" @ (%col.locked ? "locked" : "unlocked") @ "\c6.", 3);
			}
			else
				commandToClient(%obj.client, 'centerPrint', "\c6The key does not fit.", 3);
		}
	}
	
	// Player Events
	//Removes addHealth event
	function Player::addHealth(%this, %aug, %client)
	{
		if(!isObject(%client))
			parent::addHealth(%this, %aug, %client);
	}
};
RPRegPackage(RP_Core_User);

// Entering the RP
function GameConnection::EnterRPGame(%client)
{
	%key = %client.getSaveKey();
	if (RPDB.addKey(%key))
	{
		%data = RPDB.makeOnline(%key);
		if (isObject(%data))
			%client.RPData = %data;
	}
	
	// Update player name
	%name = ($Server::serverType $= "Internet") ? %client.name : %client.LANName;
	RPDB.set(%key, "name", %name);

	AddMember(%client); // Minigame
	%client.minigame = RP_MiniGame;
	
	commandToClient(%client, 'ControlRPGUI');
	
	// Check for GUI
	%client.SendModToGUI("core");
	
	%client.schedule(50, FinishRPJoining);
}

// Leaving the RP
function GameConnection::LeaveRPGame(%client)
{
	%player = %client.player;
	if (isObject(%player))
	{
		%tools = "";
		%db = %player.getDatablock();
		// Save equipments
		for (%i = 0; %i < %db.maxTools; %i++)
		{
			%tools = (%tools $= "") ? %player.tool[%i].uiName : %tools TAB %player.tool[%i].uiName;
		}
		%client.RPData.value["tools"] = %tools;
		%player.changeDataBlock(PlayerStandardArmor, %client);
	}
	// This is only for non-dedicated servers
	if (isObject(RPDB))
	{
		// Avoid multiple players
		%amount = 0;
		for (%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			%target = ClientGroup.getObject(%i);
			if (%target.RPData == %client.RPData)
				%amount++;
		}
		if (%amount <= 1)
			RPDB.makeOffline(%client.getSaveKey());
	}
	// Remove member if minigame exists(As it should do)
	RemoveMember(%client);
	// Clear RP data
	%client.minigame = "";
	%client.RPData = "";
	// Remove the bottomprint
	commandToClient(%client, 'bottomPrint', "", 1);
}

// The finish when joining the server
function GameConnection::FinishRPJoining(%client)
{
	if (!isObject(%client))
		return;
	
	%client.FinishRPSpawning();
	
	commandToClient(%client, 'DisplayRPGUI', "Stats", 1);
}

// The finish when spawning the server
function GameConnection::FinishRPSpawning(%client, %noRespawn)
{
	if (!isObject(%client))
		return;
	
	%client.updateInfo();
	
	//if(CityRPGXData.getData(%client.bl_id).valueGender $= "ambig")
	//if(%client.RPData.Value["gender"] $= "ambig")
	//{
	//	messageClient(%client, '', "\c6You have been set as a male by default. Type \c3/sexChange\c6 to be known as female.");
	//	%client.RPData.Value["gender"] = "Male";
	//	applyForcedBodyParts();
	//}
	
	//%client.gender = %client.RPData.value["gender"];
	//%client.applyForcedBodyParts();
	//%client.applyForcedBodyColors();
	
	
			
	%player = %client.player;
	
	if (isObject(%player))
	{
		%player.RP_giveDefaultEquipment();
		
		// Make some player names and standard color
		%player.setShapeNameDistance($RP::pref::server::visibleDistance);
		%player.setShapeNameColor("1 1 1 1");
	}
}

// Display additional information to player
function GameConnection::displayInfo(%client)
{
	if (!isObject(%client))
		return;
	
	%gui = %client.haveRPGUI("core");
	
	$RP::stats::info[$RP::stats::var_i["exp"]] = $RP::pref::user::displayExp;
	
	if (%gui || (!$RP::pref::user::displayBottomPrint))
		return;
	
	// Initialize variables
	%info = "<font:palatino linotype:18>";
	
	// Go through list
	for (%i = 1; %i <= $RP::stats::amount; %i++)
	{
		if($RP::stats::name[%i] $= "exp")
		{
			// Hidden value
			if ($RP::stats::hide[%i] && (%var == 0 || %var $= ""))
				continue;
			
			%group = $RP::job::group[%client.RPData.value["jobID"]];
			if(%group $= "FFFFFF")
			{
				%var  = %client.RPData.value["exp"];
				%var = getMetricPrefix(%var);
			}
			else if(%group $= "999999")
			{
				%var  = %client.RPData.value["LaborExp"];
				%var = getMetricPrefix(%var);
			}
			else if(%group $= "333333")
			{
				%var  = %client.RPData.value["CrimeExp"];
				%var = getMetricPrefix(%var);
			}
			else if(%group $= "CCCC00")
			{
				%var  = %client.RPData.value["LawExp"];
				%var = getMetricPrefix(%var);
			}
			else if(%group $= "0000CC")
			{
				%var  = %client.RPData.value["BusinessExp"];
				%var = getMetricPrefix(%var);
			}
			else if(%group $= "CC00CC")
			{
				%var  = %client.RPData.value["MedicalExp"];
				%var = getMetricPrefix(%var);
			}
			else
			{
				%var = %client.RPData.value["exp"];
				%var = getMetricPrefix(%var);
			}
			
			%info = %info @ "\c6" @ $RP::stats::name[%i] @ ": \c3" @ $RP::stats::prefix[%i] @ %var @ $RP::stats::suffix[%i] @ " ";
			%infos = (%infos $= "") ? %var : %infos NL %var;
		}
		else
		{
			if (!$RP::stats::info[%i])
				continue;
			%var = RP_GetStatVarFromData2(%i, %client.RPData);
			// Hidden value
			if ($RP::stats::hide[%i] && (%var == 0 || %var $= ""))
				continue;
			if (%var == atoi(%var))
				%var = getMetricPrefix(%var);
			%info = %info @ "\c6" @ $RP::stats::name[%i] @ ": \c3" @ $RP::stats::prefix[%i] @ %var @ $RP::stats::suffix[%i] @ " ";
			%infos = (%infos $= "") ? %var : %infos NL %var;
		}
	}
	
	if (%gui)
		commandToClient(%client, 'UpdateRPStats', %infos);
	else
		commandToClient(%client, 'bottomPrint', %info, $RP::pref::user::updateInterval + 1);
}

// Display info each second
function GameConnection::updateInfo(%client)
{
	if (!$RP::activated)
		return;
	if (!isObject(%client))
		return;
	if (!%client.isInRP())
		return;
	if (isEventPending(%client.displayTime))
		Cancel(%client.displayTime);
	
	%client.displayInfo();
	
	// Only when no gui is found
	if (!%client.haveRPGUI("core"))
		%client.displayTime = %client.schedule(1000 * $RP::pref::user::updateInterval, updateInfo);
	else
		commandToClient(%client, 'bottomPrint', "", 1);
	
	return;
}

// Display stats
function GameConnection::displayStats(%client, %ID)
{
	if (!RPDB.existKey[%ID])
		return;
	
	// GUI controls
	if (%client.haveRPGUI("Core"))
	{
		for (%i = 1; %i <= $RP::stats::amount; %i++)
		{
			%var = RP_GetStatVar(%i, %ID);
			%stats = (%stats $= "") ? %row : %stats NL %var;
		}
		commandToClient(%client, 'DisplayRPStats', %stats);
	}
	else
	{
		for (%i = 1; %i <= $RP::stats::amount; %i++)
		{
			%var = RP_GetStatVar(%i, %ID);
			%stats = %stats @ "\c6" @ $RP::stats::name[%i] @ ": \c3" @ $RP::stats::prefix[%i] @ %var @ $RP::stats::suffix[%i] @ " ";
			// New line
			if ((%i-1) % 4 == 3)
			{
				messageClient(%client, '', %stats);
				%stats = "";
			}
			else if (%i == $RP::stats::amount)
			{
				messageClient(%client, '', %stats);
			}
		}
	}
}

// Check what sort of trust
function GameConnection::CheckPrefTrust(%client, %rank)
{
	switch (%rank)
	{
		case 0: // No one
			return false;
		case 1: // But host
			return %client.isHost();
		case 2: // But Super Admins
			return %client.isSuperAdmin;
		case 3: // But Admins
			return %client.isAdmin;
		case 4: // Anyone
			return true;
		default: // Value is wrong, go to default
			return %client.isSuperAdmin;
	}
}

// Get rank level
function GameConnection::GetPrefTrust(%client)
{
	switch (true)
	{
		case %client.isHost(): // Host
			return 1;
		case %client.isSuperAdmin: // Super Admins
			return 2;
		case %client.isAdmin: // Admins
			return 3;
		default: // Anyone
			return 4;
	}
}

// Give default equipments
function Player::RP_giveDefaultEquipment(%player)
{
	%client = %player.client;
	if (!isObject(%client))
		return;
	if (!%client.isInRP())
		return;
	// Choose source tools
	%tools = (%client.RPData.value["tools"] $= "") ? "Hammer " TAB "Wrench" TAB "Printer" : %client.RPData.value["tools"];
	
	%player.addTools(%tools);
	
	%client.RPData.value["tools"] = %tools;
}

// Add tools
function Player::addTools(%player, %tools)
{
	// Create talbe if not created
	if($UiNameTableCreated == false)
		createUiNameTable();
	
	%client = %player.client;
	
	%a = 0;
	%count = getFieldCount(%tools);
	// Go throught tools
	for (%i = 0; %a < %player.getDatablock().maxTools; %i++)
	{
		if (%i < %count)
		{
			// Add tool
			%toolID = $uiNameTable_Items[getField(%tools, %i)];
			if (!isObject(%toolID))
				continue;
			%player.tool[%a] = %toolID;
			messageClient(%client, 'MsgItemPickup', "", %a, %toolID);
		}
		else
		{
			// Reset tool
			%player.tool[%a] = "";
			messageClient(%client, 'MsgItemPickup', "", %a, 0);
		}
		%a++;
	}
}
	
// Get object from point of view
function Player::GetObjectFromPOW(%player, %mask)
{
	%mask = %mask || $TypeMasks::all;
	%point = %player.getEyePoint();
	%obj = containerRayCast(%point, vectorAdd(vectorScale(vectorNormalize(%player.getEyeVector()), 4), %point), %mask, %player);
	return firstWord(%obj);
}

// Check if can do an action
function GameConnection::CheckLastAction(%client, %obj)
{
	// If passed last action time
	return !(%client.lastAction > $sim::time - $RP::pref::user::lastActionTimer);
}

// Updates the last thing done on a clickable brick
function GameConnection::ThrowLastActive(%client)
{
	if (!isObject(%client))
		return;
	// Remove old one
	if (isEventPending(%client.activateSchedule))
	{
		Cancel(%client.activateSchedule);
		%client.activateSchedule = 0;
	}
	%client.activateSchedule = %client.schedule(1000 * $RP::pref::user::stopActivate, stopActivate);
}

// Remove clickable function
function GameConnection::StopActivate(%client)
{
	if (!isObject(%client))
		return;
	// Remove old one
	if (isEventPending(%client.activateSchedule))
	{
		Cancel(%client.activateSchedule);
		%client.activateSchedule = 0;
	}
	
	%client.target = 0;
	%client.activateDatablock = 0;
	%client.activateState = 0;
}

// Get a save key depending on game mode
function GameConnection::getSaveKey(%client)
{
	switch$ ($Server::ServerType)
	{
		case "SinglePlayer":
			return %client.LANname;
		case "LAN":
			return %client.getRawIP();
		case "Internet":
			return %client.bl_id;
		// Avoid return nothing
		default:
			return %client.name;
	}
}

// Is in RP minigame
function GameConnection::isInRP(%client)
{
	return isObject(RP_MiniGame) && (%client.minigame $= RP_MiniGame || %client.minigame $= RP_MiniGame.getID());
}

// Host function
function GameConnection::isHost(%client)
{
	return %client.bl_id == getNumKeyId() || %client.getRawIP() $= "local";
}

function gameConnection::setGameBottomPrint(%client)
{
	
    if(%mainfont $= "")
	    %mainFont = "<font:BrowalliaUPC:30>";

	%client.CRPGPrint = %mainFont;
	%client.CRPGPrintBusiness = %mainFont;
	%client.CRPGPrintGang = %mainFont;
	
	%mainFont2 = "<just:right><font:BrowalliaUPC:25>";
	%client.CRPGPrintCenter = %mainFont2;

    %color = CRPGData.getData(%client.bl_id).valueLayout;    
    if(%color $= "")
        %color = "<color:3C9EFF>";
		
	if(getBusiness(CRPGData.getData(%client.bl_id).valueBusID, "Name") $= "")
	{
		CRPGData.getData(%client.bl_id).valueBusStocks = "";
	}
	
	
		
			%client.CRPGPrintBusiness = %client.CRPGPrintBusiness @ %color @ "Business Name: \c6" @ getBusiness(CRPGData.getData(%client.bl_id).valueBusID, "Name");
		
			%client.CRPGPrintBusiness = %client.CRPGPrintBusiness SPC %color @ "Account: \c6" @ getBusiness(CRPGData.getData(%client.bl_id).valueBusID, "Money");
		
			%client.CRPGPrintBusiness = %client.CRPGPrintBusiness SPC %color @ "Position: \c6" @ CRPGData.getData(%client.bl_id).valueBusPosition @ "<br>";
		
			%client.CRPGPrintBusiness = %client.CRPGPrintBusiness @ %color @ "Stocks: \c6" @ CRPGData.getData(%client.bl_id).valueBusStocks;
    	
			%client.CRPGPrintBusiness = %client.CRPGPrintBusiness SPC %color @ "Income Per Stock: \c6" @ getBusiness(CRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks")/10/10 @ "<BR>";
    	
			%client.CRPGPrintBusiness = %client.CRPGPrintBusiness SPC %color @ "Total Income: \c6" @ CRPGData.getData(%client.bl_id).valueBusStocks * getBusiness(CRPGData.getData(%client.bl_id).valueBusID, "OriginalStocks")/10/10;
    	
			%client.CRPGPrintBusiness = %client.CRPGPrintBusiness SPC %color @ "Count: \c6" @ getMembers(CRPGData.getData(%client.bl_id).valueBusID)/10/10;
			
	
		
			%client.CRPGPrintGang = %client.CRPGPrintGang @ %color @ "Name: \c6" @ getGang(CRPGData.getData(%client.bl_id).valueGangID, "Name");
		
			%client.CRPGPrintGang = %client.CRPGPrintGang SPC %color @ "Rank: \c6" @ CRPGData.getData(%client.bl_id).valueGangPosition;
		
			%client.CRPGPrintGang = %client.CRPGPrintGang SPC %color @ "<br>Storage: \c6( \c3$" @ getGang(CRPGData.getData(%client.bl_id).valueGangID, "Money") @ "\c6 | \c3" @ getGang(CRPGData.getData(%client.bl_id).valueGangID, "Lumber") @ " lumber\c6 )";
		
			%client.CRPGPrintGang = %client.CRPGPrintGang SPC %color @ "<br>Online Members: \c6" @ getGangCount(CRPGData.getData(%client.bl_id).valueGangID);
		
	
    if($Mayor::Current $= "")
        %client.CRPGPrint = %client.CRPGPrint @ "" @ %color @ "" @ %color @ "Mayor: \c6None<br>";
    else
        %client.CRPGPrint = %client.CRPGPrint @ "" @ %color @ "" @ %color @ "Mayor: \c6" SPC $Mayor::Current @ "<br>";
    
    
    
    
    
    

    
    %health = 100 - %client.player.getDamageLevel();
    if(%health > 90) 
	{
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC " " @ %color @ "Health: \c2" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 75) {
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC " " @ %color @ "Health: <color:A9F5A9>" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 50) {
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC " " @ %color @ "Health: \c6" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 30) {
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC " " @ %color @ "Health: \c3" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else if(%health > 15) {
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC " " @ %color @ "Health: <color:FA8258>" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    } else {
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC " " @ %color @ "Health: \c0" @ (100 - %client.player.getDamageLevel()) @ "\c6%<br>";
    }

    
	if(getWord(CRPGData.getData(%client.bl_id).valueJailData, 1))
		%jobName = "Convict";
	else if(CRPGData.getData(%client.bl_id).valueStudent)
		%jobName = "Student" SPC %client.getJobSO().name;
	else
		%jobName = %client.getJobSO().name;
	
	%client.CRPGPrint = %client.CRPGPrint @ "" @ %color @ "Job: \c6" @ %jobName;
	
	%client.hasDonated = 0;
	
	%client.CRPGPrint = %client.CRPGPrint SPC "" @ %color @ "Money:" SPC %client.getCashString();
	
	
	//%client.CRPGPrint = %client.CRPGPrint SPC "" @ %color @ "Income: \c6$" @ %client.getSalary() @ "(Disabled)<BR>";
	%client.CRPGPrint = %client.CRPGPrint SPC "" @ %color @ "Income: \c6$" @ %client.getSalary() @ "(-" @ %client.brickGroup.taxes @ ")<BR>";
	
	if(!(CRPGData.getData(%client.bl_id).valueDemerits >= $CRPG::pref::demerits::wantedLevel))
	{
        %client.CRPGPrint = %client.CRPGPrint SPC "" @ %color @ "Lumber: \c6" @ (strlen(getWord(CRPGData.getData(%client.bl_id).valueResources, 0)) > 0 ? getWord(CRPGData.getData(%client.bl_id).valueResources, 0) : 0);
	    %client.CRPGPrint = %client.CRPGPrint SPC "" @ %color @ "Ore: \c6" @ (strlen(getWord(CRPGData.getData(%client.bl_id).valueResources, 1)) > 0 ? getWord(CRPGData.getData(%client.bl_id).valueResources, 1) : 0);
	}

	
	if(%client.brickGroup.evidence > 0)
		%client.CRPGPrint = %client.CRPGPrint SPC "" @ %color @ "Evidence: \c6" @ %client.brickGroup.evidence;
		
	
	if(%client.brickGroup.totaldrugs > 0)
		%client.CRPGPrint = %client.CRPGPrint SPC "" @ %color @ "Drugs: \c6" @ %client.brickGroup.totaldrugs;
	
	
	%hexColor = $CRPG::food::color[%client.RPData.value["hunger"]];
	%hungerName = $CRPG::food::state[%client.RPData.value["hunger"]];
	%client.CRPGPrintCenter = %client.CRPGPrintCenter SPC "" @ %color @ "Hunger: <color:" @ %hexColor @ ">" @ %hungerName;
	
	
	if(CRPGData.getData(%client.bl_id).valueDemerits >= $CRPG::pref::demerits::wantedLevel)
	{
		%client.CRPGPrint = %client.CRPGPrint SPC %color @ "Wanted:";
		%stars = %client.getWantedLevel();
		%client.CRPGPrint = %client.CRPGPrint SPC "<color:ffff00>";
		
		for(%a = 0; %a < %stars; %a++)
			%client.CRPGPrint = %client.CRPGPrint @ "*";
		
		%client.CRPGPrint = %client.CRPGPrint @ "<color:888888>";
		for(%a = %a; %a < 6; %a++)
			%client.CRPGPrint = %client.CRPGPrint @ "*";
		
		%client.CRPGPrint = %client.CRPGPrint @ %mainFont;
	}
	
	
	if(getGang(CRPGData.getData(%client.bl_id).valueGangID, "Name"))
		%client.CRPGPrintCenter = %client.CRPGPrintCenter SPC "<br>" @ %color @ "Gang:\c6" SPC getGang(CRPGData.getData(%client.bl_id).valueGangID, "Name");
    
	
    $Economics::replayCount = $Economics::replayCount + 1;
    $Economics::randomUporDown = getRandom(1,5);
    $Economics::positiveNegative = getRandom(1,2);
    if($Economics::Relay < 1)
        $Economics::Relay = ClientGroup.getCount();

    if($Economics::replayCount > $Economics::Relay) {
        if($Economics::Condition > $Economics::Greatest) {
            $Economics::Condition = $Economics::Condition - $Economics::randomUporDown;
            $Economics::replayCount = 0;
        } else if($Economics::Condition < $Economics::Least) {
            $Economics::Condition = $Economics::Condition + $Economics::randomUporDown;
            $Economics::replayCount = 0;
        } else if($Economics::positiveNegative == 1) { 
                $Economics::Condition = $Economics::Condition + $Economics::randomUporDown;
                $Economics::replayCount = 0;
        } else if($Economics::positiveNegative == 2) { 
                $Economics::Condition = $Economics::Condition - $Economics::randomUporDown;
                $Economics::replayCount = 0;
        }
    }
    if($Economics::Condition > $Economics::Cap)
    {
        $Economics::Condition = $Economics::Cap;
    }
    if($Economics::Condition > 0) {
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC "<br>" @ %color @ "Economy:<color:00ee00>" SPC $Economics::Condition @ "\c6%";
    } else {
        %client.CRPGPrintCenter = %client.CRPGPrintCenter SPC "<br>" @ %color @ "Economy:<color:ee0000>" SPC $Economics::Condition @ "\c6%";
    }

    
    $Announcer::relayCount++;
    if($Announcer::relayCount >= $Announcer::Relay)
    {
		if($Announcer::Num == 0)
		{
			$Announcer::Message3 = "Want this addon? Download at <a:tysproductions.com>TysProductions.com</a>";
			$Announcer::Message2 = "<font:arial:20><color:ffffff>" @ $Announcer::Message3;
			messageAll('',"\c6" @ $Announcer::Message2);
			$Announcer::Num = 1;
		} else {
			messageAll('',"\c6" @ $Announcer::Message);
			$Announcer::Num = 0;
		}
        $Announcer::relayCount = 0;
    }
	
   
    $Mayor::relayCount++;
    if($Mayor::relayCount >= $Mayor::Relay)
    {
		if($Mayor::Current $= "" || $Mayor::Current $= null)
			$Mayor::Active = 0;
        automaticMayor();
        $Mayor::relayCount = 0;
    }
	
	if(!CRPGData.getData(%client.BL_ID).valueShowPrints)
	{
		if(CRPGData.getData(%client.BL_ID).valuePrint !$= "None")
		{
			if(CRPGData.getData(%client.BL_ID).valuePrint $= "Business")
				commandToClient(%client, 'bottomPrint', %client.CRPGPrintBusiness, 0, true);
			else if(CRPGData.getData(%client.BL_ID).valuePrint $= "Stats")
				commandToClient(%client, 'bottomPrint', %client.CRPGPrint, 0, true);
			else if(CRPGData.getData(%client.BL_ID).valuePrint $= "Gangs")
				commandToClient(%client, 'bottomPrint', %client.CRPGPrintGang, 0, true);
			else
				commandToClient(%client, 'bottomPrint', %client.CRPGPrint, 0, true);
			commandToClient(%client, 'centerPrint', %client.CRPGPrintCenter, 10);
		}
	}

	return %client.CRPGPrint;
}
