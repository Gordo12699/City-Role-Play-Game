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
