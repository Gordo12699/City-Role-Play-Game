// Jail Baton Item

// Datablocks
datablock ProjectileData(JailBatonProjectile : hammerProjectile)
{
	directDamage = 0;
	lifeTime = 150;
	explodeOnDeath = false;
};

datablock ItemData(JailBatonItem : hammerItem)
{
	category = "Weapon";
	className = "Weapon";
	uiName = "Jail Baton";
	canDrop = true;
	noSpawn = true;
	image = JailBatonImage;
	doColorShift = true;
	colorShiftColor = "0.2 0.2 0.00000 1.000000";
};

datablock ShapeBaseImageData(JailBatonImage : hammerImage)
{
	className = "WeaponImage";
	rotation = "0 1 0 0";
	item = JailBatonItem;
	projectile = JailBatonProjectile;
	projectileType = Projectile;
	colorShiftColor = "0.2 0.2 0.00000 1.000000";
};

// Functions

function JailBatonImage::onFire(%datablock, %obj, %slot)
{
	Parent::onFire(%datablock, %obj, %slot);
	
	%obj.playThread(2, "armAttack");
}

function JailBatonImage::onStopFire(%datablock, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function JailBatonProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.sourceObject.client;
	if (isObject(%col) && %client.isInRP())
	{
		if (%col.getType() & $TypeMasks::PlayerObjectType)
			%col.onJailBaton(%client);
	}
	serverPlay3D(hammerHitSound, %pos);
}
