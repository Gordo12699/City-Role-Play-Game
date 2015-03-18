// ============================================================
// Project				:	CityRPG
// Author				:	Iban
// Description			:	Drop-Cash-On-Death Module Code File
// ============================================================
// Table of Contents
// 1. Package Data
// 1.1. Drop Money
// 1.2. Money Pickup
// 2. Money Datablock
// ============================================================

// ============================================================
// Section 1 : Package Data
// ============================================================
package CRPG_Cash
{
	// Section 1.1 : Drop Money
	function gameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %unknownA)
	{
		if(%client.RPData.value["jail"] <= 0 && %client.RPData.value["Money"] && !%client.moneyOnSuicide)
		{
			%cash = new Item()
			{
				datablock = cashItem;
				canPickup = false;
				value = mFloor(%client.RPData.value["money"]);
			};
			
			%cash.setTransform(setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 2));
			%cash.setVelocity(VectorScale(%client.player.getEyeVector(), 10));
			
			MissionCleanup.add(%cash);
			%cash.setShapeName("$" @ %cash.value);
			
			%client.RPData.value["money"] = 0;
			%client.updateInfo();
		}
		
		parent::onDeath(%client, %killerPlayer, %killer, %damageType, %unknownA);
	}
	
	// Section 1.2 : Money Pickup
	function Armor::onCollision(%this, %obj, %col, %thing, %other)
	{
		if(%col.getDatablock().getName() $= "CashItem")
		{
			if(isObject(%obj.client))
			{
				if(isObject(%col))
				{
					if(%obj.client.minigame)
						%col.minigame = %obj.client.minigame;
					
					%obj.client.RPData.value["Money"] += %col.value;
					messageClient(%obj.client, '', "\c6You have picked up \c3$" @ %col.value SPC "\c6off the ground.");
					
					%obj.client.RPData.value["CrimeExp"] += 1;
					%obj.client.updateInfo();
					%col.canPickup = false;
					%col.delete();
				}
				else
				{
					%col.delete();
					MissionCleanup.remove(%col);
				}
			}
		}
		
		if(isObject(%col))
			parent::onCollision(%this, %obj, %col, %thing, %other);
	}
	
	function CashItem::onAdd(%this, %item, %b, %c, %d, %e, %f, %g)
	{
		parent::onAdd(%this, %item, %b, %c, %d, %e, %f, %g);
		schedule($CRPG::pref::moneyDieTime, 0, "eval", "if(isObject(" @ %item.getID() @ ")) { " @ %item.getID() @ ".delete(); }");
	}
};
activatePackage(CRPG_Cash);

// ============================================================
// Section 2 : Money Datablock
// ============================================================
datablock ItemData(cashItem)
{
	category = "Weapon";
	className = "Weapon";
	
	shapeFile = "base/data/shapes/brickWeapon.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	doColorShift = true;
	doColorShift = false;
	colorShiftColor = "0 0.6 0 1";
	image = cashImage;
	candrop = true;
	canPickup = false;
};

datablock ShapeBaseImageData(cashImage)
{
	shapeFile = "base/data/shapes/brickWeapon.dts";
	emap = true;
	
	doColorShift = true;
	doColorShift = false;
	colorShiftColor = cashItem.colorShiftColor;
	canPickup = false;
};
