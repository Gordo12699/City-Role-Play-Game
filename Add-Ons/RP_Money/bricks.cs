// --------------------------
// RP Money :: Bricks
// --------------------------

if (isPackage(RP_Money_Bricks))
	deactivatePackage(RP_Money_Bricks);

package RP_Money_Bricks
{
	function serverCmdPlantBrick(%client)
	{
		%datablock = %client.player.tempbrick.getDatablock();
		// Check admin
		if (!%client.isAdmin && %datablock.adminOnly)
		{
			commandToClient(%client, 'centerPrint', "\c3You must be admin to plant this brick!", 3);
			return false;
		}
		
		Parent::serverCmdPlantBrick(%client);
	}
	
	// RP Plant Brick
	function fxDTSBrick::PlantBrick(%brick)
	{
		Parent::PlantBrick(%brick);
		
		if (!isObject(%brick))
			return;
		
		%brickGroup = getBrickGroupFromObject(%brick);
		%client = %brickGroup.client;
		%datablock = %brick.getDatablock();
		%initPrice = mFloor(%datablock.cost);
		
		%isSafe = (%datablock.uiName $= "Safe");
		
		
		
		// No owner
		if (!isObject(%client))
		{
			if (%isSafe)
				%brickGroup.haveSafe = 1;
			return;
		}
		
		if (%isSafe)
		{
			// Already have a safe
			if (%brickGroup.haveSafe)
			{
				commandToClient(%client, 'centerPrint', "\c3You can only have one safe.", 3);
				%brick.schedule(0, "delete");
				return;
			}
		}
		
		// The brick cost to plant
		if (%initPrice > 0)
		{
			if (%client.isInRP())
			{
				if (%client.RPData.value["money"] < %initPrice)
				{
					messageClient(%client, '', '\c6You need at least \c3%1\c6 in order to plant this \c3%2\c6.', prepareDisplayMoney(%initPrice), %datablock.uiName);
					%brick.schedule(0, "delete");
					return;
				}
			
				%client.RPData.value["money"] -= %initPrice;
				messageClient(%client, '', '\c6You have paid \c3%1\c6 to plant this \c3%2\c6.', prepareDisplayMoney(%initPrice), %datablock.uiName);
				%client.displayInfo();
				// Safe
				if (%isSafe)
				{
					%brickGroup.haveSafe = 1;
					%brick.isSafe = 1;
				}
			}
		}
		// Brick cost money
		else if ($RP::pref::server::brickCost > 0)
		{
			if (%client.isInRP())
			{
				if (%client.RPData.value["money"] >= $RP::pref::server::brickCost)
				{
					%brick.schedule(500, "brickPlanted");
					if(brickPlanted(%brick))
					{
						%client.RPData.value["money"] -= $RP::pref::server::brickCost;
						%client.displayInfo();
					}
				}
				else
				{
					messageClient('', '\c6You need at least \c3%1\c6 in order to plant this \c3%2\c6.', prepareDisplayMoney($RP::pref::server::brickCost), %datablock.uiName);
					%brick.schedule(0, "delete");
					return;
				}
			}
		}
		return;
	}
	
	// RP Loading Brick
	function fxDTSBrick::LoadingBrick(%brick)
	{
		Parent::LoadingBrick(%brick);
		
		if (!isObject(%brick))
			return false;
		
		%brickGroup = getBrickGroupFromObject(%brick);
		%datablock = %brick.getDatablock();
		
		if (%datablock.uiName $= "Safe")
		{
			%brickGroup.haveSafe = 1;
			%brick.isSafe = 1;
		}
	}
	
	// RP Remove Brick
	function fxDTSBrick::RemoveBrick(%brick)
	{
		%brickGroup = getBrickGroupFromObject(%brick);
		Parent::RemoveBrick(%brick);
		if (%brick.getDatablock().uiName $= "Safe")
			%brickGroup.haveSafe = 0;
	}
	
	function TimeBombFinalExplosionProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
	{
		%damageType = $DamageType::Direct;
		if(%this.DirectDamageType)
			%damageType = %this.DirectDamageType;

		%scale = getWord(%obj.getScale(), 2);
		%directDamage = mClampF(%this.directDamage, -100, 100) * %scale;

		if(%col.getType() & $TypeMasks::PlayerObjectType)
		{
		  %col.damage(%obj, %pos, %directDamage, %damageType);
		}
		else if(%col.getDatablock().uiName $= "Bank Brick")
		{
			messageAll('', "\c3The bank has been robbed!");
		}
		else
		{
			%col.damage(%obj, %pos, %directDamage, %damageType);
		}
	}
};
RPRegPackage(RP_Money_Bricks);

function brickPlanted(%brick)
{
	if(isObject(%brick))
	{
		return true;
	}
	else
	{
		return false;
	}
}
