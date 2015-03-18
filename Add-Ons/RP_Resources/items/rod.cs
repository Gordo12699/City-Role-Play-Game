// Rod Item

// Datablocks
datablock ProjectileData(RodProjectile : hammerProjectile)
{
	directDamage = 0;
	lifeTime = 150;
	explodeOnDeath = false;
};

datablock ItemData(RodItem : hammerItem)
{
	category = "Weapon";
	className = "Weapon";
	uiName = "Rod";
	canDrop = true;
	noSpawn = true;
	image = RodImage;
	doColorShift = true;
	colorShiftColor = "0.300000 0.200000 0.100000 1.000000";
};

datablock ShapeBaseImageData(RodImage : hammerImage)
{
	className = "WeaponImage";
	rotation = "0 1 0 0";
	item = RodItem;
	projectile = RodProjectile;
	projectileType = Projectile;
	colorShiftColor = "0.300000 0.200000 0.100000 1.000000";
};

// Functions

function RodImage::onFire(%datablock, %obj, %slot)
{
	Parent::onFire(%datablock, %obj, %slot);
	
	%obj.playThread(2, "armAttack");
}

function RodImage::onStopFire(%datablock, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function RodProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.sourceObject.client;
	if (isObject(%client) && %client.isInRP() && %col.getType() & $TypeMasks::fxBrickObjectType)
		%col.onFish(%client);
	serverPlay3D(hammerHitSound, %pos);
}
