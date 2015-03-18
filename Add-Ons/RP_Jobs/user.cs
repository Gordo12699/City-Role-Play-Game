// --------------------------
// RP Jobs :: User
// --------------------------

if (isPackage(RP_Jobs_User))
	deactivatePackage(RP_Jobs_User);

package RP_Jobs_User
{
	// Activating
	function Player::activateStuff(%player)
	{
		Parent::activateStuff(%player);
		
		// Retreive target
		%obj = %player.GetObjectFromPOW($TypeMasks::PlayerObjectType);
		%client = %player.client;
		if (!%client.CheckLastAction(%obj))
			return 0;
		
		if (!isObject(%obj))
			return 0;
		
		// Player
		if (%obj.getType() & $TypeMasks::PlayerObjectType)
		{
			%target = %obj.client;
			%special = $RP::job::special[%client.RPData.value["jobID"]];
			
			// Healing
			if (firstWord(%special) $= "heal")
			{
				%max = getWord(%special, 1);
				%amount = getRandom(1, %max);
				%obj.addHealth(%client, 0, %amount, 0);
				messageClient(%target, '', '\c3%1\c6 has healed you. \c2+%2 health', %client.name, %amount);
				%target.RPData.value["MedicalExp"] += 1;
				%client.lastAction = $sim::time;
			}
		}
		
		return 1;
	}
	
	// RP Core
	// RP Core
	function GameConnection::FinishRPSpawning(%client, %noRespawn)
	{
		Parent::FinishRPSpawning(%client, %noRespawn);
		
		if (!isObject(%client))
			return;
		
		%player = %client.player;
		if (!isObject(%player))
			return;
		
		// Choose datablock
		%datablock = $RP::job::datablock[%client.RPData.value["jobID"]];
		if (%datablock !$= "")
		{
			%client.RP_PlayerDatablock = %datablock;
			%player.changeDataBlock(%datablock, %client);
		}
		
		if (!%noRespawn)
		{
			// Tourist spawn ( Standard spawn )
			if (%client.RPData.value["jobID"] == 1)
			{
				//if (%tSpawn = FindSpawn("jobSpawn", 1))
					//%spawn = %tSpawn;
				
				for (%i = 0; %i < BrickGroup_20829.getCount(); %i++)
				{
					%datablock = BrickGroup_20829.getObject(%i).getDatablock();
					%brick = BrickGroup_20829.getObject(%i);
		
					if (%datablock.isSpawnBrick)
					{
						if(%datablock.spawnData $= "jobSpawn 1")
						{
							%spawn = %brick.getTransform();
							%TouristSpawn = %brick.getTransform();
						}
					}
				}
			}
			
			if(%client.RPData.value["jobID"] != 1)
			{
				// Job spawn
				//if (%tSpawn = FindSpawn("jobSpawn", %client.RPData.value["jobID"]))
					//%spawn = %tSpawn;
				
				for (%i = 0; %i < BrickGroup_20829.getCount(); %i++)
				{
					%datablock = BrickGroup_20829.getObject(%i).getDatablock();
					%brick = BrickGroup_20829.getObject(%i);
		
					if (%datablock.isSpawnBrick)
					{
						if(%datablock.spawnData $= "jobSpawn " @ %client.RPData.value["jobID"])
						{
							%spawn = %brick.getTransform();
							if(%spawn $= "")
							{
								%spawn = %touristSpawn;
							}
						}
					}
				}
			}
			
			%brickGroup = "BrickGroup_" @ %client.BL_ID;
			// Personal spawn
			//if (%tSpawn = FindSpawn("personalSpawn", %client))
			//	%spawn = %tSpawn;
			for (%i = 0; %i < %brickGroup.getCount(); %i++)
			{
				%datablock = %brickGroup.getObject(%i).getDatablock();
				%brick = %brickGroup.getObject(%i);
		
				if (%datablock.isSpawnBrick)
				{
					if(%datablock.spawnData $= "personalSpawn")
					{
						if(%client.bl_id == %brick.client.bl_id)
						{
							%spawn = %brick.getTransform();
						}
					}
				}
			}
			
			
			// Jail spawn
			if (RPModExist("Crime") && %client.RPData.value["jail"] > 0)
			{
				//if (%tSpawn = FindSpawn("jailSpawn"))
				//	%spawn = %tSpawn;
				
				for (%i = 0; %i < BrickGroup_20829.getCount(); %i++)
				{
					%datablock = BrickGroup_20829.getObject(%i).getDatablock();
					%brick = BrickGroup_20829.getObject(%i);
			
					if (%datablock.isSpawnBrick)
					{
						if(%datablock.spawnData $= "jobSpawn 26")
						{
							%spawn = %brick.getTransform();
						}
					}
				}
			}
			
			// Transfer player
			if (%spawn !$= "")
				%player.setTransform(%spawn);
		}
		
		// Recolor for group
		%group = $RP::job::group[%client.RPData.value["jobID"]];
		if (%group !$= "")
		{
			if($RP::pref::server::localChat)
			{
				%player.setShapeNameDistance(0);
				%player.setShapeNameColor("1 1 1 1");
			}
			else
			{
				%player.setShapeNameDistance(150);
				%player.setShapeNameColor("1 1 1 1");
			}
			%player.setShapeNameColor(HexToRGBA(%group));
		}
	}
	
	function WheeledVehicleData::onCollision(%this, %obj, %col, %pos, %vel)
	{
		if(%obj.locked && %col.getType() & $TypeMasks::PlayerObjectType && isObject(%col.client))
		{
			commandToClient(%col.client, 'centerPrint', "\c6The vehicle is locked.", 3);
		}
		else if(isObject(%obj.spawnBrick) && %obj.spawnBrick.getDatablock().getName() $= "CRPGCrimeVehicleData" && isObject(%col.client) && !$RP::job::useCrimeCars[%col.client.RPData.value["jobID"]])
		{
			commandToClient(%col.client, 'centerPrint', "\c6This vehicle is a criminal vehicle.", 3);
		}
		else if(isObject(%obj.spawnBrick) && %obj.spawnBrick.getDatablock().getName() $= "CRPGPoliceVehicleData" && isObject(%col.client) && !$RP::job::usePoliceCars[%col.client.RPData.value["jobID"]])
		{
			commandToClient(%col.client, 'centerPrint', "\c6This vehicle is property of the Police Department.", 3);
		}
		else
		{
			parent::onCollision(%this, %obj, %col, %pos, %vel);
		}
	}
};
RPRegPackage(RP_Jobs_User);

// Give default equipments for the job
// Overwrote the function for inheritence to work
function Player::RP_giveDefaultEquipment(%player)
{
	%client = %player.client;
	if (!isObject(%client))
		return;
	// Choose source tools
	%tools = (%client.RPData.value["tools"] $= "") ? $RP::job::tools[%client.RPData.value["jobID"]] : %client.RPData.value["tools"];
	
	%player.addTools(%tools);
	
	%client.RPData.value["tools"] = %tools;
}
