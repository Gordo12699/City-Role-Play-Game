// Pickaxe Item with model from Lilboarders RPGTools

package RPG_PickaxePackage
{
	function rpgPickaxeProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
	{
		%client = %obj.sourceObject.client;
		if (isObject(%client) && %client.isInRP() && %col.getType() & $TypeMasks::fxBrickObjectType)
		{
			%col.onMine(%client);
			return;
		}
		if (%col.getType() & $TypeMasks::PlayerObjectType && isObject(%client))
			return;
		Parent::onCollision(%projectile, %obj, %col, %fade, %pos, %normal);
	}
};
RPRegPackage(RPG_PickaxePackage);
