// Lockpick Item

// Datablocks
datablock ProjectileData(PicklockProjectile : hammerProjectile)
{
	directDamage = 0;
	lifeTime = 100;
	explodeOnDeath = false;
};

datablock ItemData(PicklockItem : hammerItem)
{
	category = "Weapon";
	className = "Weapon";
	uiName = "Lockpick";
	canDrop = true;
	noSpawn = true;
	image = PicklockImage;
	doColorShift = true;
	colorShiftColor = "0.600000 0.000000 0.000000 1.000000";
};

datablock ShapeBaseImageData(PicklockImage : hammerImage)
{
	className = "WeaponImage";
	rotation = "0 1 0 180";
	item = PicklockItem;
	projectile = PicklockProjectile;
	projectileType = Projectile;
	colorShiftColor = "0.600000 0.000000 0.000000 1.000000";
};

// Functions

function PicklockImage::onFire(%datablock, %obj, %slot)
{
	Parent::onFire(%datablock, %obj, %slot);
	
	%obj.playThread(2, "armAttack");
}

function PicklockImage::onStopFire(%datablock, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function PicklockProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.sourceObject.client;
	if (isObject(%col) && %client.isInRP())
	{
		if (%col.getType() & $TypeMasks::fxBrickObjectType)
			%col.onPicklock(%client);
	}
}
