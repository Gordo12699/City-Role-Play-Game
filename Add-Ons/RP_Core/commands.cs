// --------------------------
// RP Core :: Commands
// --------------------------

function serverCmdSaveCity(%client)
{
	talk("Saving to saves//NewCityRPSAVE.bls ...");
	talk($Server::BrickCount @ " bricks saved to saves//NewCityRPSAVE.bls successfully.");
	saveBricks("NewCityRPSAVE", 1, 1);
}

function serverMessageRotation(%this)
{	
	echo("Server Message - Sent");
	messageAll('', "\c2CRPG\c6: Be sure to stay updated with everything happening on the server by joining the <a:steamcommunity.com/groups/GCRPG>Steam Group</a>\c6!");
	schedule(15 * 60000, 0, serverMessageRotation);
}

// Startup command
function ServerCmdStartRP(%client)
{
	if ($RP::activated)
		return;
	if (!%client.isHost())
		return;
	// Please load everything already
	if (!%client.hasSpawnedOnce)
		return;
	messageAll('', "\c3Creating RP...");
	RPActivatePackages();
	BootRP();
	
	schedule(1000, 0, serverMessageRotation);
	$Pref::Duplorcator::AdminOnly = false;
}

// Shutdown command
function ServerCmdEndRP(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.isHost())
		return;
	// Please load everything already
	if (!%client.hasSpawnedOnce)
		return;
	messageAll('', "\c3Removing RP...");
	ShutRP();
	RPDeactivatePackages();
}

// Joinging rp
function ServerCmdJoinRP(%client)
{
	if (!$RP::activated)
		return;
	if (!isObject(RP_MiniGame))
	{
		CreateMiniGame();
		// Users joining the RP
		%count = ClientGroup.getCount();
		for (%i = 0; %i < %count; %i++)
			ClientGroup.getObject(%i).EnterRPGame();
	}
	if (%client.isInRP())
		return;
	ServerCmdJoinMiniGame(%client, RP_MiniGame.getID());
	%client.EnterRPGame();
}

// Leaving rp
function ServerCmdLeaveRP(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.isSuperAdmin)
		return;
	if (!%client.isInRP())
		return;
	ServerCmdLeaveMiniGame(%client);
}

// Package

if (isPackage(RP_Core_Commands))
	deactivatePackage(RP_Core_Commands);

package RP_Core_Commands
{
	// Fetch client commands and messages
	function ServerCmdMessageSent(%client, %text)
	{
		if (!%client.isInRP())
		{
			Parent::ServerCmdMessageSent(%client, %text);
			return;
		}
		// Activates a datablock
		if (isObject(%client.activateDatablock))
		{
			// Exit
			if (%text $= "0")
			{
				%client.StopActivate();
				messageClient(%client, '', "\c6Thank you come again.");
				return;
			}
			
			// Info brick
			if (%client.activateDatablock.isInfoBrick)
			{
				if (!%client.activateDatablock.parseData(%client, %text))
					messageClient(%client, '', "\c3Invalid command");
			}
			// Just clear some variables if you happen to get here
			else
			{
				%client.StopActivate();
				commandToClient(%client, 'centerPrint', "\c2This clickable brick is broken.", 2);
			}
			%client.displayInfo();
			return;
		}
		// Handle old command
		if (getSubStr(%text, 0, 1) $= "^" && getSubStr(%text, 1, 2) !$= "^" && getSubStr(%text, 1, 2) !$= "." && getSubStr(%text, 1, 2) !$= "_")
		{
			messageClient(%client, '', "\c6This is not CityRP with the \c3^\c6 command. Use \c3/\c6 instead.");
			return;
		}
		// All-Caps filter
		if ($RP::pref::server::capsFilter && strlen(%text) > 10 && strCmp(strUpr(%text), %text) == 0)
		{
			messageClient(%client, '', "\c6Please be friendly to Blockheads.");
			return;
		}
		
		// Local chat
		if ($RP::pref::server::localChat && isObject(%client.player))
		{
			%heard = 0;
			// Amount of players in the area
			InitContainerRadiusSearch(%client.player.getPosition(), $RP::pref::server::visibleDistance, $TypeMasks::PlayerObjectType);
			while((%search = ContainerSearchNext()) != 0)
				%heard++;

			%color = %heard > 1 ? "\c2" : "\c0";
			
			// Send the message
			InitContainerRadiusSearch(%client.player.getPosition(), $RP::pref::server::visibleDistance, $TypeMasks::PlayerObjectType);
			while((%search = ContainerSearchNext()) != 0)
				messageClient(%search.client, 'chatMessage', '%1[Local] \c7%2\c3%3\c7%4\c6: %5', %color, %client.clanPrefix, %client.name, %client.clanSuffix, %text);
		}
		else
		{
			Parent::ServerCmdMessageSent(%client, %text);
		}
	}
	
	// Team messages
	function ServerCmdTeamMessageSent(%client, %text)
	{
		if (isObject(RP_MiniGame) && $RP::pref::server::localChat && isObject(%client.player) && %client.isInRP())
		{
			messageAll('', '\c7%1\c3%2\c7%3\c6: %4', %client.clanPrefix, %client.name, %client.clanSuffix, %text);
		}
		else
		{
			Parent::ServerCmdTeamMessageSent(%client, %text);
		}
	}
	
	function serverCmdDropCameraAtPlayer(%client)
	{
		if (%client.isInRP())
		{
			return false;
		}
		
		Parent::serverCmdDropCameraAtPlayer(%client);
	}
	
	function serverCmdDropPlayerAtCamera(%client)
	{
		if (%client.isInRP())
		{
			return false;
		}
		
		Parent::serverCmdDropPlayerAtCamera(%client);
	}
};
RPRegPackage(RP_Core_Commands);

// Update the user stats
function ServerCmdUpdateInfo(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	%client.updateInfo();
}

// Toggle the big letters function
function ServerCmdToggleCapsFilter(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	// Super Admins only
	if (!%client.isSuperAdmin)
		return;
	$RP::pref::server::capsFilter = !$RP::pref::server::capsFilter;
	messageAll('', '\c6All-Caps Filter is now \c3%1\c6.', ($RP::pref::server::capsFilter ? "active" : "inactive"));
}

// Toggle report
function ServerCmdToggleReport(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	// Super Admins only
	if (!%client.isSuperAdmin)
		return;
	$RP::pref::server::report = !$RP::pref::server::report;
	messageAll('', '\c6The report system is now \c3%1\c6.', ($RP::pref::server::report ? "active" : "inactive"));
}

// Toggle suggestion
function ServerCmdToggleSuggest(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	// Super Admins only
	if (!%client.isSuperAdmin)
		return;
	$RP::pref::server::suggest = !$RP::pref::server::suggest;
	messageAll('', '\c6Suggestions are now \c3%1\c6.', ($RP::pref::server::suggest ? "active" : "inactive"));
}

// Toggle local chat
function ServerCmdToggleLocalChat(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	// Super Admins only
	if (!%client.isSuperAdmin)
		return;
	$RP::pref::server::localChat = !$RP::pref::server::localChat;
	messageAll('', '\c6Local chat is now \c3%1\c6.', ($RP::pref::server::localChat ? "active" : "inactive"));
}

// Set local chat range
function ServerCmdSetLocalChatRange(%client, %text)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	// Super Admins only
	if (!%client.isSuperAdmin)
		return;
	$RP::pref::server::visibleDistance = (mFloor(%text) >= 10) ? mFloor(%text) : 10;
	// Update distance for each player
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		if (isObject(%client.player))
			%client.player.setShapeNameDistance($RP::pref::server::visibleDistance);
	}
	messageAll('', '\c6Local chat range is now \c3%1\c6.', $RP::pref::server::visibleDistance);
}

// Admin chat
function ServerCmdAdmin(%client, %v0, %v1, %v2, %v3, %v4, %v5, %v6, %v7, %v8, %v9, %v10, %v11, %v12, %v13, %v14, %v15, %v16, %v17, %v18, %v19, %v20)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	if (!%client.isAdmin)
		return;
	// Prepare text
	%text = trim(%v0 SPC %v1 SPC %v2 SPC %v3 SPC %v4 SPC %v5 SPC %v6 SPC %v7 SPC %v8 SPC %v9 SPC %v10 SPC %v11 SPC %v12 SPC %v13 SPC %v14 SPC %v15 SPC %v16 SPC %v17 SPC %v18 SPC %v19 SPC %v20);
	
	// Empty text
	if (%text $= "")
		return;
	
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%target = ClientGroup.getObject(%i);
		if (%target.isAdmin)
			messageClient(%target, 'chatMessage', '\c3[\c0Admin\c3] %1: \c7%2', %client.name, %text);
	}
}

// Report
function ServerCmdReport(%client, %v0, %v1, %v2, %v3, %v4, %v5, %v6, %v7, %v8, %v9, %v10, %v11, %v12, %v13, %v14, %v15, %v16, %v17, %v18, %v19, %v20)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	if (!$RP::pref::server::report)
	{
		messageClient(%client, '', "\c6The report system is currently inactive.");
		return;
	}
	
	// Prepare text
	%text = trim(%v0 SPC %v1 SPC %v2 SPC %v3 SPC %v4 SPC %v5 SPC %v6 SPC %v7 SPC %v8 SPC %v9 SPC %v10 SPC %v11 SPC %v12 SPC %v13 SPC %v14 SPC %v15 SPC %v16 SPC %v17 SPC %v18 SPC %v19 SPC %v20);
	// Empty text
	if (%text $= "")
	{
		messageClient(%client, '', "\c6Please use the following format:\c3 /report <\c0insert text here\c3>");
		messageClient(%client, '', "\c6Example:\c3 /report I can't plant bricks.");
		return;
	}
	
	// Add name, id and the report itself
	if (AppendToFile("report", %client.name @ ", " @ %client.getSaveKey() SPC "::" SPC %text))
		messageClient(%client, '', "\c6Report sent successfully.");
}

// Suggest
function ServerCmdSuggest(%client, %v0, %v1, %v2, %v3, %v4, %v5, %v6, %v7, %v8, %v9, %v10, %v11, %v12, %v13, %v14, %v15, %v16, %v17, %v18, %v19, %v20)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	if (!$RP::pref::server::suggest)
	{
		messageClient(%client, '', "\c6Suggestions are currently inactive.");
		return;
	}
	
	// Prepare text
	%text = trim(%v0 SPC %v1 SPC %v2 SPC %v3 SPC %v4 SPC %v5 SPC %v6 SPC %v7 SPC %v8 SPC %v9 SPC %v10 SPC %v11 SPC %v12 SPC %v13 SPC %v14 SPC %v15 SPC %v16 SPC %v17 SPC %v18 SPC %v19 SPC %v20);
	// Empty text
	if (%text $= "")
	{
		messageClient(%client, '', "\c6Please use the following format:\c3 /suggest <\c0insert text here\c3>");
		messageClient(%client, '', "\c6Example:\c3 /suggest I would like to have a beach.");
		return;
	}
	
	// Add name, id and the suggestion itself
	if (AppendToFile("suggestions", %client.name @ ", " @ %client.getSaveKey() SPC "::" SPC %text))
		messageClient(%client, '', "\Suggestion sent successfully.");
}

// Get stats
function ServerCmdStats(%client, %arg0, %arg1, %arg2, %arg3)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	// Closed
	if (!$RP::pref::server::stats)
		return false;
	
	if (!%client.isSuperAdmin)
	{
		%client.displayStats(%client.getSaveKey());
		return false;
	}
	
	%name = trim(%arg0 SPC %arg1 SPC %arg2 SPC %arg3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		// Is numeric
		if (%arg0 $= atoi(%arg0))
			%ID = %arg0;
		// It is you
		else
			%ID = %client.getSaveKey();
	}
	else
		%ID = %target.getSaveKey();
	
	%client.displayStats(%ID);
	return true;
	%haveGUI = %client.haveRPGUI("Core");
	
	// User exists in database
	if (RPDB.existKey[%ID])
	{
		// GUI controls
		if (%haveGUI)
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
		return true;
	}
	
	messageClient(%client, '', "\c6User does not exist.");
	return false;
}

///////////////////////
// Advanced commands //
///////////////////////

// Grant experience
function ServerCmdGrantExp(%client, %amount, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	
	%amount = mFloor(%amount);
	if (%amount <= 0)
	{
		messageClient(%client, '', "\c6Invalid amount.");
		return;
	}
	
	%name = trim(%n0 SPC %n1 SPC %n2 SPC %n3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		messageClient(%client, '', "\c6Invalid player.");
		return;
	}
	
	// Not enough rank but not yourself
	if (%target != %client && %target.GetPrefTrust() <= %client.GetPrefTrust())
	{
		messageClient(%client, '', '\c3%1\c6 has higher rank than you. Unable to grant experience.', %target.name);
		return;
	}
	
	RPDB.set(%target.getSaveKey(), "exp", RPDB.get(%target.getSaveKey(), "exp") + %amount);
	
	messageClient(%client, '', '\c6You granted \c3%1\c6 experience to \c3%2\c6.', %amount, %target.name);
	messageClient(%target, '', '\c3%1\c6 granted you \c3%2\c6 experience.', %client.name, %amount);
	
	%target.displayInfo();
}

// Deduct experience
function ServerCmdDeductExp(%client, %amount, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	
	%amount = mFloor(%amount);
	if (%amount <= 0)
	{
		messageClient(%client, '', "\c6Invalid amount.");
		return;
	}
	
	%name = trim(%n0 SPC %n1 SPC %n2 SPC %n3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		messageClient(%client, '', "\c6Invalid player.");
		return;
	}
	
	// Not enough rank but not yourself
	if (%target != %client && %target.GetPrefTrust() <= %client.GetPrefTrust())
	{
		messageClient(%client, '', '\c3%1\c6 has higher rank than you. Unable to deduct experience.', %target.name);
		return;
	}
	
	RPDB.set(%target.getSaveKey(), "exp", RPDB.get(%target.getSaveKey(), "exp") - %amount);
	
	messageClient(%client, '', '\c6You deducted \c3%1\c6 experience from \c3%2\c6.', %amount, %target.name);
	messageClient(%target, '', '\c3%1\c6 deducted \c3%2\c6 experience from you.', %client.name, %amount);
	
	%target.displayInfo();
}

// Clear everything
function ServerCmdClearPlayer(%client, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	if (!%client.isInRP())
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	
	%name = trim(%n0 SPC %n1 SPC %n2 SPC %n3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		messageClient(%client, '', "\c6Invalid player.");
		return;
	}
	
	// Not enough rank but not yourself
	if (%target != %client && %target.GetPrefTrust() <= %client.GetPrefTrust())
	{
		messageClient(%client, '', '\c3%1\c6 has higher rank than you. Unable to clear user data.', %target.name);
		return;
	}
	
	%target.RPData.clear();
	
	messageClient(%client, '', '\c6You cleared \c3%1\c6\'s user data.', %target.name);
	messageClient(%target, '', '\c3%1\c6 cleared your user data..', %client.name);
	
	// Respawn player
	if (isObject(%target.player))
		%target.player.delete();
	%target.spawnPlayer();
	
	%target.displayInfo();
}

//package CRPG_SuicidePackage
//{
//	function serverCmdSuicide(%client)
//	{
			// Respawn player
//		if (isObject(%client.player))
//		{
//			if(%client.player.tempbrick)
//			{
//				%client.player.tempbrick.delete();
//			}
//			
//			%client.player.delete();
//		}
//		
//		%client.spawnPlayer();
//	}
//};
//activatePackage(CRPG_SuicidePackage);

//Rules command
function serverCmdrules(%client)
{
	messageClient(%client,'',"\c6- \c3Use the categories as /commands to access any help category.\c6 -");
	messageClient(%client,'',"\c31\c6: /serverrules - Breaking these rules will result in a 1-Mute, 2-Kick, 3-Ban");
	messageClient(%client,'',"\c32\c6: /buildingrules - Breaking these rules will result in 1-Flag, 2-Destroyed Bricks, 3-Destroyed Lot, 4-Ban");
	messageClient(%client,'',"\c33\c6: /miscrules - Gives information on how to report badmins, or becoming an admin.");
	messageClient(%client,'',"\c6Most important of all, use common sense. If something sounds bad, don't do it.");
	messageClient(%client,'',"\c6An admin can ban you for anything reasonable if it's not listed.");
	//messageClient(%client,'',"\c33\c6: /difficultyhelp - Gives information on difficulty settings and how they affect gameplay.");
}

function serverCmdserverrules(%client)
{
	messageClient(%client,'',"\c31\c6: No spamming chat or excessive caps. 1");
	messageClient(%client,'',"\c32\c6: No abusing bugs. Report all bugs to Gordo12699 or any Admin. 3");
	messageClient(%client,'',"\c33\c6: No spamming or abusing to lag or break the server. 2,3");
	messageClient(%client,'',"\c34\c6: Respect everyone. No unneeded hate or flaming. 1,2,3");
	messageClient(%client,'',"\c35\c6: No spawn killing or spawn jailing. 2,3");
	messageClient(%client,'',"\c36\c6: No ghost killing or ghost jailing (unless they left to avoid being jailed). 2,3");
	messageClient(%client,'',"\c37\c6: No baiting. This includes walking infront of weapons or standing in the street. 2,3");
	messageClient(%client,'',"\c38\c6: No being annoying. Rapid hammeriing, wanding, painting, etc. 2,3");
	messageClient(%client,'',"\c39\c6: No leaving the city limits to avoid being jailed. 2,3");
	messageClient(%client,'',"\c310\c6: No scamming. Scamming will result in your bank money being reduced. If you were dumb enough to fall for a scam, you may not be refunded. 2,3");
	messageClient(%client,'',"\c311\c6: No abusing trust. 2,3");
	messageClient(%client,'',"\c312\c6: No multiclienting or alting. 2,3");
}

function serverCmdbuildingrules(%client)
{
	messageClient(%client,'',"\c31\c6: Place lots inside of the 16*16 grid inside of the city. 3");
	messageClient(%client,'',"\c32\c6: WIP lots can't be inactive for more than 4 days. 1,3");
	messageClient(%client,'',"\c33\c6: Finished lots can't be inactive for more than 8 days. 1,3");
	messageClient(%client,'',"\c34\c6: No box builds. A sloped roof does not count. 1,3");
	messageClient(%client,'',"\c35\c6: No spammy bricks. 1*1*5 walls, 2*1 brick walls, etc. 1,3");
	messageClient(%client,'',"\c36\c6: No more than 1 music brick per city block. 2");
	messageClient(%client,'',"\c37\c6: No excessive emitters, lights, or FX. 2");
	messageClient(%client,'',"\c38\c6: No abusive, spammy, or laggy events. 1,2,3,4");
	messageClient(%client,'',"\c39\c6: No lot reserving. Start building on a lot as soon as you place it. 1,3");
	messageClient(%client,'',"\c310\c6: Industrial lots must be in the industrial area. 3");
	messageClient(%client,'',"\c311\c6: No innapropriate builds. 1,2,3,4");
	messageClient(%client,'',"\c312\c6: No single color builds. 1,3");
	messageClient(%client,'',"\c312\c6: No vehicle spawns on roofs. Keep designs realistic. 1,3");
}

function serverCmdmiscrules(%client)
{
	messageClient(%client,'',"\c31\c6: If a player disconnects to avoid being jailed, you can ghost-jail them next time they spawn.");
	messageClient(%client,'',"\c32\c6: Getting damaged from a vehicle explosion doesn't give anyone demerits. If you see a vehicle on fire, run.");
	messageClient(%client,'',"\c33\c6: It is impossible to pickpocket inside of an admin lot.");
	messageClient(%client,'',"\c34\c6: Admins are not allowed to DM.");
	messageClient(%client,'',"\c35\c6: Admins are not allowed to cheat.");
	messageClient(%client,'',"\c36\c6: Harboring criminals will allow an admin to add demerits to your account.");
}

//Help command
// function servercmdhelp(%client)
// {
// messageClient(%client,'',"\c6- \c3Use the categories as /commands to access any help category.\c6 -");
	// messageClient(%client,'',"\c31\c6: /brickhelp - Gives information on building and bricks.");
	// messageClient(%client,'',"\c32\c6: /jobhelp - Gives information on jobs and how to obtain them.");
	// messageClient(%client,'',"\c33\c6: /adminhelp - Gives information on how to report badmins, or becoming an admin.");
	// messageClient(%client,'',"\c33\c6: /difficultyhelp - Gives information on difficulty settings and how they affect gameplay.");
// }

//brickhelp command
function servercmdbrickhelp(%client)
{
messageClient(%client,'',"\c6- \c3Brick Help");
	messageClient(%client,'',"\c31\c6: To start your build, you must use a lot from the CityRP[G] Brick tab.");
	messageClient(%client,'',"\c32\c6: Spamming is against the rules and will result in immediate destruction of your build");
	messageClient(%client,'',"\c33\c6: Do not build anything that could offend or repulse other players, it will result in immediate destruction of the entire build.");
	messageClient(%client,'',"\c34\c6: Depending on the color of the brick, you will either be spending ores, lumber, or money to build.");
	messageClient(%client,'',"\c35\c6: If you need additional help please look towards an admin for advice.");
}

//Jobhelp command
function servercmdjobhelp(%client)
{
messageClient(%client,'',"\c6- \c3Job Help");
	messageClient(%client,'',"\c31\c6: The jobs are located in a Library. Look for that to begin your career.");
	messageClient(%client,'',"\c32\c6: To get a job go to the purple-screened monitor and click it, then use the default chat (usually accessed by the 't' key) to access sections corresponding to the numbers.");
	messageClient(%client,'',"\c33\c6: If you would like to see any new jobs placed in the Employment Office, use the /suggest (suggestion) command.");
	messageClient(%client,'',"\c34\c6: Certain jobs only get paid during certain hours of the day, remember this when looking for a comfortable-paying job");
	messageClient(%client,'',"\c35\c6: Certain jobs get different items to help commit to the job, choose one you like best!");
	messageClient(%client,'',"\c36\c6: If you need additional help please look towards an admin for advice.");
}

//adminHelp command
function servercmdadminhelp(%client)
{
messageClient(%client,'',"\c6- \c3Admin Help");
	messageClient(%client,'',"\c31\c6: You can use the /report command to report a badmin to the host, the badmin won't even know!");
	messageClient(%client,'',"\c32\c6: Becoming an admin is a procedure that must be committed to and comitting to it even when no one is around.");
	messageClient(%client,'',"\c33\c6: If you are looking to become admin be sure to spread the server around the Blockland community and visit the server frequently.");
	messageClient(%client,'',"\c34\c6: Be nice to every player, it may not earn you admin, but it may help you become noticed by any admin or server host.");
	messageClient(%client,'',"\c35\c6: If you need additional help please look towards an admin for advice.");
}

function serverCmdDebug(%client, %arg)
{
	if(%arg $= "true")
	{
		$RP::pref::server::debug = 1;
	}
	else
		$RP::pref::server::debug = 0;
}

function ServerCmdHelp(%client, %section, %type)
{
	if(true)
	{
		%client.lastCommand = $Sim::Time;
		if(%section $= "")
		{
			MessageClient(%client,'', "\c6Say \c3/Help\c6 followed by one of the sections below for information on that section.<lmargin:0>");
			MessageClient(%client,'', "\c3Jobs<lmargin:100>\c6Displays a list of jobs and their specifications.<lmargin:0>");
			MessageClient(%client,'', "\c3Items<lmargin:100>\c6Displays a list of items and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Vehicles<lmargin:100>\c6Displays a list of vehicles and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Lots<lmargin:100>\c6Displays a list of lots and their prices.<lmargin:0>");
			MessageClient(%client,'', "\c3Commands<lmargin:100>\c6Displays a list of commands.<lmargin:0>");
			MessageClient(%client,'', "\c3Credits<lmargin:100>\c6Displays a list of credits for Gordo12699's City RPG.<lmargin:0>");

			//if(%client.isAdmin)
				//MessageClient(%client,'', "\c3Admin<lmargin:100>\c6Displays a list of admin only commands.<lmargin:0>");
		}
		else if(%section $= "list")
		{
			MessageClient(%client,'', "\c3Gordo12699- \c6The mod you are playing has an organized job list.");
			MessageClient(%client,'', "\c3Gordo12699- \c6To view the job category types type /help jobs");
			MessageClient(%client,'', "\c3Gordo12699- \c6To search a category type: /help jobs [category]");
			MessageClient(%client,'', "\c3Example: \c6/help jobs law");
		}
		else if(%section $= "Jobs")
		{
			if(%type $= "")
			{
				MessageClient(%client,'', "\c6Here are the \c3types\c6 of jobs:");
				MessageClient(%client,'', "\c3Civilian<lmargin:100>\c6List all \c3default\c6 and \c3starting\c6 jobs.<lmargin:0>");
				MessageClient(%client,'', "\c3Labor<lmargin:100>\c6List all jobs that deal with gathering \c3resources\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Business<lmargin:100>\c6List all jobs dealing with running \c3shops\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Law Enforcement<lmargin:100>\c6List all jobs that are in \c3law enforcement\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Medical<lmargin:100>\c6List all \c3medical\c6 jobs.<lmargin:0>");	
				MessageClient(%client,'', "\c3Criminal<lmargin:100>\c6List all jobs that are for \c3criminals\c6.<lmargin:0>");
				MessageClient(%client,'', "\c3Politics<lmargin:100>\c6List all jobs dealing with \c3politics\c6.<lmargin:0>");
				//MessageClient(%client,'', "\c3Donator<lmargin:100>\c6List all jobs that are for \c3donators\c6 and \c3sponsors\c6.<lmargin:0>");
				//MessageClient(%client,'', "\c3Admin<lmargin:100>\c6List all jobs that are for \c3admins\c6.<lmargin:0>");			
			} 
			else if(%type $= "all")
			{
				MessageClient(%client,'', "\c6Here is a \c3list\c6 of all the jobs:");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
					@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID]
					@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID]
					@ "\c6 Base Salary: \c3" @ $RP::job::pay[%ID]
					@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
					@ "\c6 Education: \c3" @ $RP::job::education[%ID]
					@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
					@ "\c6 Sell Food: \c3" @ YN($RP::job::sellFood[%ID])
					@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
					@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
					@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
					@ "<lmargin:0>");
					messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
					messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @ "<lmargin:0>");
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "Civilian")
			{
				MessageClient(%client,'', "\c6Here is a list of all \c3default\c6 and \c3starting\c6 jobs.<lmargin:0>");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					if($RP::job::group[%ID] $= "FFFFFF")
					{
						messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
						@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID] 
						@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID] 
						@ "\c6 Base Salary: \c3" @ $RP::job::pay[%ID]
						@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
						@ "\c6 Education: \c3" @ $RP::job::education[%ID]
						@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Food: \c3" @ YN($RP::job::sellFood[%ID])
						@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
						@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
						@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
						messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @ "<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "Labor")
			{
				MessageClient(%client,'', "\c6Here is a list of all jobs that deal with gathering \c3resources\c6.<lmargin:0>");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					if($RP::job::group[%ID] $= "999999")
					{
						messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
						@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID] 
						@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID] 
						@ "\c6 Base Salary: \c3" @ $RP::job::pay[%ID]
						@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
						@ "\c6 Education: \c3" @ $RP::job::education[%ID]
						@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Food: \c3" @ YN($RP::job::sellFood[%ID])
						@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
						@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
						@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
						messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @ "<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "Business")
			{
				MessageClient(%client,'', "\c6Here is a list of all jobs dealing with running \c3shops\c6.<lmargin:0>");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					if($RP::job::group[%ID] $= "0000CC")
					{
						messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
						@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID] 
						@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID] 
						@ "\c6 Base Salary: \c3" @ $RP::job::pay[%ID]
						@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
						@ "\c6 Education: \c3" @ $RP::job::education[%ID]
						@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Food: \c3" @ YN($RP::job::sellFood[%ID])
						@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
						@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
						@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
						messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @ "<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "Medical")
			{
				MessageClient(%client,'', "\c6Here is a list of all \c3medical\c6 jobs.<lmargin:0>");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					if($RP::job::group[%ID] $= "CC00CC")
					{
						messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
						@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID] 
						@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID]
						@ "\c6 Pay: \c3" @ $RP::job::pay[%ID]
						@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
						@ "\c6 Education: \c3" @ $RP::job::education[%ID]
						@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Food: \c3" @ YN($RP::job::sellFood[%ID])
						@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
						@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
						@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
						messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @ "<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "Law Enforcement")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are in \c3law enforcement\c6.<lmargin:0>");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					if($RP::job::group[%ID] $= "CCCC00")
					{
						messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
						@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID] 
						@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID]
						@ "\c6 Pay: \c3" @ $RP::job::pay[%ID]
						@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
						@ "\c6 Education: \c3" @ $RP::job::education[%ID]
						@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Food: \c3" @ YN($RP::job::sellFood[%ID])
						@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
						@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
						@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
						messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @"<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "Criminal")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3criminals\c6.<lmargin:0>");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					if($RP::job::group[%ID] $= "333333")
					{
						messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
						@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID] 
						@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID]
						@ "\c6 Pay: \c3" @ $RP::job::pay[%ID]
						@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
						@ "\c6 Education: \c3" @ $RP::job::education[%ID]
						@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Food: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
						@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
						@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
						messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @"<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "Politics")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3polictics\c6.<lmargin:0>");
				for (%i = 1; %i <= $RP::jobs; %i++)
				{
					%ID = $RP::job::listID[%i];
					if($RP::job::group[%ID] $= "CCCC00")
					{
						messageClient(%client, '', "\c3" @ $RP::job::name[%ID] @ "<lmargin:150>" 
						@ "\c3-\c6Experience: \c3" @ $RP::job::experience[%ID]
						@ "\c6 Career Experience: \c3" @ $RP::job::careerExperience[%ID]
						@ "\c6 Pay: \c3" @ $RP::job::pay[%ID] 
						@ "\c6 Pay Time: \c3" @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true)
						@ "\c6 Education: \c3" @ $RP::job::education[%ID]
						@ "\c6 Sell Items: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Food: \c3" @ YN($RP::job::sellItems[%ID])
						@ "\c6 Sell Services: \c3" @ YN($RP::job::sellServices[%ID])
						@ "\c6 Pickpocket: \c3" @ YN($RP::job::thief[%ID])
						@ "\c6 Pardon: \c3" @ YN($RP::job::pardon[%ID])
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ $RP::job::tools[%ID]);
						messageClient(%client, '', "<lmargin:150>\c3---"@ $RP::job::info[%ID] @"<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "donator")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3donators\c6 and \c3sponsors\c6.<lmargin:0>");
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "donator" || JobSO.job[%a].type $= "sponsor")
					{
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ "<lmargin:150>" 
						@ "\c3-\c6Investment: \c3" @ JobSO.job[%a].invest 
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay 
						@ "\c6 Education: \c3" @ JobSO.job[%a].education
						@ "\c6 Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3---"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
			else if(%type $= "admin")
			{
				MessageClient(%client,'', "\c6Here is a list all jobs that are for \c3admins\c6.<lmargin:0>");
				for(%a = 1; %a <= JobSO.getJobCount(); %a++)
				{
					if(JobSO.job[%a].type $= "admin" || JobSO.job[%a].type $= "mod")
					{
						messageClient(%client, '', "\c3" @ JobSO.job[%a].name @ "<lmargin:150>" 
						@ "\c3-\c6Investment: \c3" @ JobSO.job[%a].invest 
						@ "\c6 Pay: \c3" @ JobSO.job[%a].pay 
						@ "\c6 Education: \c3" @ JobSO.job[%a].education
						@ "\c6 Sell Items: \c3" @ YN(JobSO.job[%a].sellItems)
						@ "\c6 Sell Food: \c3" @ YN(JobSO.job[%a].sellFood)
						@ "\c6 Sell Services: \c3" @ YN(JobSO.job[%a].sellServices)
						@ "\c6 Pickpocket: \c3" @ YN(JobSO.job[%a].thief)
						@ "\c6 Pardon: \c3" @ YN(JobSO.job[%a].canPardon)
						@ "<lmargin:0>");
						messageClient(%client, '', "<lmargin:150>\c3--\c6Items: \c3" @ JobSO.job[%a].tools);
						messageClient(%client, '', "<lmargin:150>\c3---"@ JobSO.job[%a].helpline @"<lmargin:0>");
					}
				}
				//MessageClient(%client,'', "\c6Say \c3/Job\c6 followed by one of the jobs above to become that job.");
			}
		}
		else if(%section $= "Items")
		{
			MessageClient(%client,'', "\c6Here is a list of items and their prices.");
			for(%c = 1; %c <= $ListAmt-1; %c++)
			{
				MessageClient(%client,'', "\c3" @ $CRPG::prices::weapon::name[%c].uiName @ "<lmargin:150>\c6Price: <color:33cc33>$" @ $CRPG::prices::weapon::price[%c] @ "<lmargin:0>");
			}
		}
		else if(%section $= "Vehicles")
		{
			MessageClient(%client,'', "\c6Here is a list of vehicles and their prices.");

			for(%c = 1; %c <= $vehicleListAmt-1; %c++)
			{
				MessageClient(%client,'', "\c3" @ $CRPG::prices::vehicle::name[%c].uiName @ "<lmargin:150>\c6Price: <color:33cc33>$" @ $CRPG::prices::vehicle::price[%c] @ "<lmargin:0>");
			}
				//MessageClient(%client,'', "\c3"@ $CRP::Vehicle[%a] @"<lmargin:150>\c6Price: \c3$"@ $CRP::Vehicle::Price[$CRP::Vehicle[%a]] @"<lmargin:0>");
		}
		else if(%section $= "Lots")
		{
			MessageClient(%client,'', "\c6Here is a list of lots you can plant.");
			MessageClient(%client,'', "\c6Small House<lmargin:100>\c6Cost[\c3$500\c6] Tax[\c3$10\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Half Medium House<lmargin:100>\c6Cost[\c3$750\c6] Tax[\c3$15\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Medium House<lmargin:100>\c6Cost[\c3$1500\c6] Tax[\c3$25\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Large House<lmargin:100>\c6Cost[\c3$4500\c6] Tax[\c3$60\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Small Store<lmargin:100>\c6Cost[\c3$800\c6] Tax[\c3$15\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Half Medium Store<lmargin:100>\c6Cost[\c3$1300\c6] Tax[\c3$20\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Medium Store<lmargin:100>\c6Cost[\c3$2800\c6] Tax[\c3$30\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6Large Store<lmargin:100>\c6Cost[\c3$5100\c6] Tax[\c3$70\c6]<lmargin:0>");
			
		}
		else if(%section $= "Commands")
		{
			MessageClient(%client,'', "\c6Here is a list of commands.");
			MessageClient(%client,'', "\c6/Givemoney<lmargin:100>\c6[\c3Amount\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Dropmoney<lmargin:100>\c6[\c3Amount\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Trade");
			MessageClient(%client,'', "\c6/Pardon<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Adv");
			MessageClient(%client,'', "\c6/Reset<lmargin:0>");
		}
		else if(%section $= "Credits")
		{
			MessageClient(%client,'', "\c6Here is a list of credits for Gordo12699's City RPG.");
			//MessageClient(%client,'', "\c3Ty<lmargin:100>\c6Host & Lead Coder<lmargin:0>");
			//MessageClient(%client,'', "\c3Chris!<lmargin:100>\c6Lead Builder<lmargin:0>");
			//MessageClient(%client,'', "\c3Meggey<lmargin:100>\c6Brick Designer & Builder<lmargin:0>");
			//MessageClient(%client,'', "\c3Xenos109<lmargin:100>\c6HUD & GUI Designer<lmargin:0>");
			//MessageClient(%client,'', "\c3Rarw Muffinz<lmargin:100>\c6Coder<lmargin:0>");
			//MessageClient(%client,'', "\c3Moppy<lmargin:100>\c6Created Drugs<lmargin:0>");
			//MessageClient(%client,'', "\c3IBan<lmargin:100>\c6Base Developer<lmargin:0>");
		}
		else if(%section $= "Adminss" && %client.isAdmin)
		{
			MessageClient(%client,'', "\c6Here is a list of admin only commands.");
			MessageClient(%client,'', "\c6/Set<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Add<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/Deduct<lmargin:100>\c6[\c3Data\c6] [\c3Amount\c6] [\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/SetHour<lmargin:100>\c6[\c3Hour\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/DayTime");
			MessageClient(%client,'', "\c6/NextHour");
			MessageClient(%client,'', "\c6/Mute<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
			MessageClient(%client,'', "\c6/GetMuted");
			MessageClient(%client,'', "\c6/UnMute<lmargin:100>\c6[\c3Target\c6]<lmargin:0>");
		}
		else
			MessageClient(%client,'', "\c6Help section \c3"@ %section @"\c6 not found.");
	}
	else
		MessageClient(%client,'',"\c6You must wait before using this command again.");
}

function serverCmdWelcomeBox(%client)
{
	if(!isObject(%client.player))
		return;
	%message = "Check out these webpages:" NL "<a:steamcommunity.com/groups/GCRPG#announcements>CityRPG Announcements</a>" NL " <a:cityrpg.site50.net\help.php>CityRPG First-Time Guide</a>" NL "" NL "Please do not place Gun-vending-Machines or mediocre food stands.";
	
	commandToClient(%client, 'messageBoxOK', "Welcome Message", %message);
}

function serverCmdstats(%client, %name)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin && %name !$= "")
		%target = findClientByName(%name);
	else
		%target = %client;
	
	if(isObject(%target))
	{
		%string = "Career:" SPC "\c3" @ JobSO.job[CityRPGData.getData(%target.bl_id).valueJobID].name;
		%string = %string @ "\n" @ "Money in Wallet:" SPC "\c3" @ %target.RPData.value["money"];
		%string = %string @ "\n" @ "Net Worth:" SPC "\c3" @ (%target.RPData.value["money"] + %target.RPData.value["bank"]);
		%string = %string @ "\n" @ "Arrest Record:" SPC "\c3" @ (%target.RPData.value["jail"] ? "Yes" : "No");
		%string = %string @ "\n" @ "Ticks left in Jail:" SPC "\c3" @ %target.RPData.value["jail"];
		%string = %string @ "\n" @ "Total Demerits:" SPC "\c3" @ %target.RPData.value["demerits"];
		%string = %string @ "\n" @ "Education:" SPC "\c3" @ %target.RPData.value["education"];
		
		commandToClient(%client, 'MessageBoxOK', %target.name, %string);
	}
	else
		messageClient(%client, '', "\c6Either you did not enter a person or the person specified does not exist.");
}

function serverCmdScoreType(%client, %typeE)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		if(%typeE $= "" || %typeE $= null)
		{
			messageClient(%client, '', "\c6Types are: Money, Edu");
		} else {
			if(%typeE $= "Money")
			{
				$Score::typeE = "Money";
			} else if(%typeE $= "Edu") {
				$Score::typeE = "Edu";
			} else {
				$Score::typeE = "Money";
			}
		}
	}
}

function serverCmdupdateScore(%client)
{if(!isObject(%client.player)) 
		return;
	if(%client.isAdmin)
	{
		for(%d = 0; %d < ClientGroup.getCount(); %d++)
		{
			%subClient = ClientGroup.getObject(%d);
			gameConnection::setScore(%subClient, %score);
		}
		messageClient(%client, '', "\c6You've updated the score.");
	} else {
		messageClient(%client, '', "\c6Must be admin to use this command.");
	}
}
