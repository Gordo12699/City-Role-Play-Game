// Pickaxe Item

// Datablocks
datablock ProjectileData(PickaxeProjectile : hammerProjectile)
{
	directDamage = 0;
	lifeTime = 150;
	explodeOnDeath = false;
};

datablock ItemData(PickaxeItem : hammerItem)
{
	category = "Weapon";
	className = "Weapon";
	uiName = "Pickaxe";
	canDrop = true;
	noSpawn = true;
	image = PickaxeImage;
	doColorShift = true;
	colorShiftColor = "0.600000 0.600000 0.600000 1.000000";
};

datablock ShapeBaseImageData(PickaxeImage : hammerImage)
{
	className = "WeaponImage";
	rotation = "0 1 0 0";
	item = PickaxeItem;
	projectile = PickaxeProjectile;
	projectileType = Projectile;
	colorShiftColor = "0.600000 0.600000 0.600000 1.000000";
};

// Functions

function PickaxeImage::onFire(%datablock, %obj, %slot)
{
	Parent::onFire(%datablock, %obj, %slot);
	
	%obj.playThread(2, "armAttack");
}

function PickaxeImage::onStopFire(%datablock, %obj, %slot)
{
	%obj.playThread(2, "root");
}

function PickaxeProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
{
	%client = %obj.sourceObject.client;
	if (isObject(%client) && %client.isInRP() && %col.getType() & $TypeMasks::fxBrickObjectType)
		%col.onMine(%client);
	serverPlay3D(hammerHitSound, %pos);
}
