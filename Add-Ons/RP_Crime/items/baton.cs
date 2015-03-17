// Baton Item

// Datablocks
datablock ProjectileData(BatonProjectile : hammerProjectile)
{
	directDamage = 0;
	lifeTime = 150;
	explodeOnDeath = false;
};

datablock ItemData(BatonItem : hammerItem)
{
	category = "Weapon";
	className = "Weapon";
	uiName = "Baton";
	canDrop = true;
	noSpawn = true;
	image = BatonImage;
	doColorShift = true;
	colorShiftColor = "0.898039 0.898039 0.00000 1.000000";
};

datablock ShapeBaseImageData(BatonImage : hammerImage)
{
	className = "WeaponImage";
	rotation = "0 1 0 0";
	item = BatonItem;
	projectile = BatonProjectile;
	projectileType = Projectile;
	colorShiftColor = "0.898039 0.898039 0.00000 1.000000";
};

// Functions

function BatonImage::onFire(%datablock, %obj, %slot)
{
	Parent::onFire(%datablock, %obj, %slot);
	
	%obj.playThread(2, "armAttack");
}

function BatonImage::onStopFire(%datablock, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function BatonProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.sourceObject.client;
	// May only be used when the RP is online
	if (isObject(%col) && isObject(%client) && %client.isInRP())
	{
		// Do stuff on player
		if (%col.getType() & $TypeMasks::PlayerObjectType)
		{
			%col.onUseBaton(%client);
			%col.onJailBaton(%client);
		}
		
		// Do stuff on brick
		else if (%col.getType() & $TypeMasks::fxBrickObjectType)
		{
			// Open door
			if (isObject(%col.shape))
				%col.onBashDoor(%client);
			// Open safe
			else if (%col.isSafe)
				%col.onOpenSafe(%client);
			else if (%col.getDatablock().isDrug)
			{
				%col.bagPlant(%col);
				commandToClient(%client,'centerPrint',"\c6You have received \c3$100 \c6for this plant, in your Bank Account.",3);
				%bank = %client.RPData.value["Bank"];
				%client.RPData.value["Bank"] = %bank + 100;
				//echo("Drugs Taken");
			}
		}
	}
	serverPlay3D(hammerHitSound, %pos);
}

function BatonImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
{
	if(%col.getClassName() $= "fxDTSBrick")
	{
		%client = %obj.client;
		%brickData = %col.getDatablock();
		
		// Way out of date.. doesn't work.
		if(isObject(%col.door) && %col.door.closed)
		{
			%col.fakeKillBrick(%vec, 3, %client);
			%col.doorDestroy();
		}
		else if(%brickData.isDrug)
		{
			%col.bagPlant(%col);
			commandToClient(%client,'centerPrint',"\c6You have received \c3$100 \c6for this plant, in your Bank Account.",3);
			%bank = %client.RPData.value["Bank"];
			%client.RPData.value["Bank"] = %bank + 100;
			//echo("Drugs Taken");
		}
	}
	//else if(%col.getClassName() $= "Player")
	//{
	//	%client = %obj.client;
	//	if((%col.getType() & $typeMasks::playerObjectType) && isObject(%col.client))
	//	{
	//		if(%col.client.getWantedLevel())
	//		{
	//			if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
	//			{
	//				%col.setDamageLevel(%this.raycastDirectDamage + 1);
	//				%col.client.arrest(%client);
	//			}
	//			else
	//				commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6has resisted arrest!", 3);
	//		}
	//		else if(((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth) >= $CityRPG::pref::demerits::wantedLevel)
	//		{
	//			%col.client.getWantedLevel();
	//			if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
	//			{
	//				CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
	//				%col.setDamageLevel(%this.raycastDirectDamage + 1);
	//				%col.client.arrest(%client);
	//				CityRPGData.getData(%col.client.bl_id).valuemarijuana -= CityRPGData.getData(%col.client.bl_id).valuemarijuana;
	//				CityRPGData.getData(%col.client.bl_id).valuetotaldrugs -= CityRPGData.getData(%col.client.bl_id).valuetotaldrugs;
	//			}
	//			else
	//			{
	//				commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is carrying drugs, and is resisting arrest!", 3);
	//				if(CityRPGData.getData(%col.client.bl_id).valueDemerits < $CityRPG::pref::demerits::wantedLevel)
	//				{
	//					CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
	//				}
	//			}
	//		}
	//		else if(CityRPGData.getData(%col.client.bl_id).valueBounty > 0)
	//			commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is not wanted alive.", 3);
	//		else
	//			%doNoEvil = true;
	//	}
	//}
	
	//if(%doNoEvil) { %this.raycastDirectDamage = 10; }
	//parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
	//if(%doNoEvil) { %this.raycastDirectDamage = 25; }
}
