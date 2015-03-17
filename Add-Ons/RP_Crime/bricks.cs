// --------------------------
// RP Crime :: Bricks
// --------------------------

if (isPackage(RP_Crime_Bricks))
	deactivatePackage(RP_Crime_Bricks);

package RP_Crime_Bricks
{
	// These functions is only to block public access to some bricks while wanted
	
	// Employment brick
	function JobBrickData::Activate(%datablock, %client)
	{
		if (%client.RPData.value["demerits"] > $RP::pref::demerits::maximum)
		{
			%client.stopActivate();
			messageClient(%client, '', "\c6You cannot use public services.");
			return true;
		}
		Parent::Activate(%datablock, %client);
	}
	// Bank brick
	function BankBrickData::Activate(%datablock, %client)
	{
		if (%client.RPData.value["demerits"] > $RP::pref::demerits::maximum)
		{
			%client.stopActivate();
			messageClient(%client, '', "\c6You cannot use public services.");
			return true;
		}
		Parent::Activate(%datablock, %client);
	}
	
	// RP Plant Brick
	function fxDTSBrick::PlantBrick(%brick)
	{
		Parent::PlantBrick(%brick);
		
		%brickGroup = getBrickGroupFromObject(%brick);
		%client = %brickGroup.client;
		
		if (%client.RPData.value["jail"] > 0)
		{
			commandToClient(%client, 'centerPrint', "\c3You cannot build in jail!", 3);
			%brick.schedule(0, "delete");
			return false;
		}
	}
	
	function BankBrickData::parseData(%datablock, %client, %text)
	{
		%stolen = %client.RPData.value["stolenMoney"];
		if (%client.activateState != 4.2 || %stolen == 0)
		{
			return Parent::parseData(%datablock, %client, %text);
		}
		
		%target = %client.RPtarget;
		// Get span
		%money1 = %client.RPData.value["bank"];
		%t = Parent::parseData(%datablock, %client, %text);
		%money2 = %client.RPData.value["bank"];
		// No transferring
		if (%money1 == %money2 || (!isObject(%target)) || (!%t))
			return %t;
		
		// Do transfer of dirty money
		%dirty = %money1 - %money2;
		%dirty = (%dirty > %stolen) ? %stolen : %dirty;
		%client.RPData.value["stolenMoney"] -= %dirty;
		%target.RPData.value["stolenMoney"] += %dirty;
		return %t;
	}
};
RPRegPackage(RP_Crime_Bricks);
