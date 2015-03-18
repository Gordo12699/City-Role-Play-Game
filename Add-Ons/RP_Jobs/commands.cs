// --------------------------
// RP Jobs :: Commands
// --------------------------

if (isPackage(RP_Jobs_Commands))
	deactivatePackage(RP_Jobs_Commands);

package RP_Jobs_Commands
{
	// Fetch client commands and messages
	function ServerCmdMessageSent(%client, %text)
	{
		// Promotion
		if (isEventPending(%client.promotion))
		{
			Promote(%client, %text);
			return;
		}
		Parent::ServerCmdMessageSent(%client, %text);
	}
	
	// Team messages
	function ServerCmdTeamMessageSent(%client, %text)
	{
		if (isObject(RP_MiniGame))
		{
			MessageGroup(%client, %text);
		}
		else
		{
			Parent::ServerCmdTeamMessageSent(%client, %text);
		}
	}
};
RPRegPackage(RP_Jobs_Commands);

// Message in group
function ServerCmdGroup(%client, %v0, %v1, %v2, %v3, %v4, %v5, %v6, %v7, %v8, %v9, %v10, %v11, %v12, %v13, %v14, %v15, %v16, %v17, %v18, %v19, %v20)
{
	if (!$RP::activated)
		return;
	// Prepare text
	%text = trim(%v0 SPC %v1 SPC %v2 SPC %v3 SPC %v4 SPC %v5 SPC %v6 SPC %v7 SPC %v8 SPC %v9 SPC %v10 SPC %v11 SPC %v12 SPC %v13 SPC %v14 SPC %v15 SPC %v16 SPC %v17 SPC %v18 SPC %v19 SPC %v20);
	// Empty text
	if (%text $= "")
		return;
	
	MessageGroup(%client, %text);
}

// Message in group from GUI
function ServerCmdGroupMessageSent(%client, %text)
{
	if (!$RP::activated)
		return;
	// Empty text
	if (%text $= "")
		return;
	MessageGroup(%client, %text);
}

// Message all in a group
function MessageGroup(%client, %text)
{
	if (%text $= "")
		return;
	
	if (!isObject(%client))
		return;
	%job = %client.RPData.value["jobID"];
	%group = $RP::job::group[%job];
	
	// Sending to group
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%target = ClientGroup.getObject(%i);
		if ($RP::job::group[%target.RPData.value["jobID"]] $= %group)
			messageClient(%target, '', '\c3[<color:%1>%2\c3] %3: \c7%4', %group, $RP::job::name[%job], %client.name, %text);
	}
}

///////////////////////
// Advanced commands //
///////////////////////

// Change job
function ServerCmdSetJob(%client, %jobID, %n0, %n1, %n2, %n3)
{
	if (!$RP::activated)
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	
	%jobID = atoi(%jobID);
	if (%jobID <= 0)
	{
		messageClient(%client, '', "\c6Invalid job.");
		return;
	}
	if ($RP::job::name[%jobID] $= "")
	{
		messageClient(%client, '', "\c6Invalid job ID.");
		return;
	}
	
	%name = trim(%n0 SPC %n1 SPC %n2 SPC %n3);
	%target = (%name $= "") ? %client : findClientByText(%name);
	
	if (!isObject(%target))
	{
		messageClient(%client, '', "\c6Invalid player.");
		return;
	}
	// Avoid spam
	if (%target.RPData.value["jobID"] == %jobID)
	{
		messageClient(%client, '', '\c3%1\c6 already has that job.', %target.name);
		return;
	}
	
	%target.RPData.value["jobID"] = %jobID;
	
	messageClient(%client, '', '\c6You set \c3%1\c6\'s job to \c3%2\c6.', %target.name, $RP::job::name[%jobID]);
	messageClient(%target, '', '\c3%1\c6 changed your job to \c3%2\c6.', %client.name, $RP::job::name[%jobID]);
	
	%target.displayInfo();
}

function serverCmdYes(%client)
{
	if(isObject(%client.player))
	{
		if(isObject(%client.player.serviceOrigin))
		{
			if(mFloor(VectorDist(%client.player.serviceOrigin.getPosition(), %client.player.getPosition())) < 16)
			{
				if(%client.RPData.value["Money"] >= %client.player.serviceFee)
				{
					%ownerBL_ID = %client.player.serviceOrigin.getGroup().bl_id;
					switch$(%client.player.serviceType)
					{
						case "service":
							%client.RPData.value["Money"] -= %client.player.serviceFee;
							%client.player.serviceOrigin.getGroup().RPData.value["Bank"] += %client.player.serviceFee;
						
							messageClient(%client, '', "\c6You have accepted the service fee of \c3$" @ %client.player.serviceFee @ "\c6!");
							%client.setInfo();
						
							if(%client.player.serviceOrigin.getGroup().client)
								messageClient(%client.player.serviceOrigin.getGroup().client, '', "\c3" @ %client.name @ "\c6 has wired you \c3$" @ %client.player.serviceFee @ "\c6 for a service.");
							%client.player.serviceOrigin.onTransferSuccess(%client);
						
						case "food":
							%client.RPsellFood(%ownerBL_ID, %client.player.serviceSize, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);
						
						case "item":
							%client.RPsellItem(%ownerBL_ID, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);
						
						case "zone":
							%client.sellZone(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceFee);
						
						case "clothes":
							%client.sellClothes(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceItem, %client.player.serviceFee);
					}
				}
				else
				{
					messageClient(%client, '', "\c6You cannot afford this service.");
				}
			}
			else
			{
				messageClient(%client, '', "\c6You are too far away from the service to purchase it!");
			}
		}
		else
		{
			messageClient(%client, '', "\c6You have no active tranfers that you may accept!");
		}
	}
	
	%client.player.serviceType = "";
	%client.player.serviceFee = "";
	%client.player.serviceMarkup = "";
	%client.player.serviceItem = "";
	%client.player.serviceSize = "";
	%client.player.serviceOrigin = "";
}

function serverCmdNo(%client)
{
	if(isObject(%client.player.serviceOrigin))
	{
		messageClient(%client, '', "\c6You have rejected the service fee!");
		
		%client.player.serviceOrigin.onTransferDecline(%client);
		
		%client.player.serviceType = "";
		%client.player.serviceFee = "";
		%client.player.serviceMarkup = "";
		%client.player.serviceItem = "";
		%client.player.serviceSize = "";
		%client.player.serviceOrigin = "";
	}
	else
		messageClient(%client, '', "\c6You have no active tranfers that you may decline!");
}

function serverCmdReloadSpawns(%client)
{
	echo("Reloading spawns...");
	talk(%client.name SPC "reloaded spawns");
	talk("RP_Jobs: Reloading spawns...");
	
	for (%i = 0; %i < BrickGroup.getCount(); %i++)
	{
		%datablock = BrickGroup.getObject(%i).getDatablock();
		%brick = BrickGroup.getObject(%i);
		
		if (%datablock.isSpawnBrick)
			%brick.AddSpawn();
		
		%brick.ResetResource();
	}
}
