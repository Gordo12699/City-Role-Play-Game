// Axe Item

// Datablocks
datablock ProjectileData(AxeProjectile : hammerProjectile)
{
	directDamage = 0;
	lifeTime = 150;
	explodeOnDeath = false;
};

datablock ItemData(AxeItem : hammerItem)
{
	category = "Weapon";
	className = "Weapon";
	uiName = "Axe";
	canDrop = true;
	noSpawn = true;
	image = AxeImage;
	doColorShift = true;
	colorShiftColor = "0.300000 0.200000 0.100000 1.000000";
};

datablock ShapeBaseImageData(AxeImage : hammerImage)
{
	className = "WeaponImage";
	rotation = "0 1 0 0";
	item = AxeItem;
	projectile = AxeProjectile;
	projectileType = Projectile;
	colorShiftColor = "0.300000 0.200000 0.100000 1.000000";
};

// Functions

function AxeImage::onFire(%datablock, %obj, %slot)
{
	Parent::onFire(%datablock, %obj, %slot);
	
	%obj.playThread(2, "armAttack");
}

function AxeImage::onStopFire(%datablock, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function AxeProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.sourceObject.client;
	if (isObject(%client) && %client.isInRP() && %col.getType() & $TypeMasks::fxBrickObjectType)
		%col.onCut(%client);
	serverPlay3D(hammerHitSound, %pos);
}
