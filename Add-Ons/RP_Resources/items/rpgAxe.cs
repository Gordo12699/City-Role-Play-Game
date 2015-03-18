// Axe Item with model from Lilboarders RPGTools

package RPG_AxePackage
{
	function rpgAxeProjectile::onCollision(%projectile, %obj, %col, %fade, %pos, %normal)
	{
		%client = %obj.sourceObject.client;
		if (isObject(%client) && %client.isInRP() && %col.getType() & $TypeMasks::fxBrickObjectType)
		{
			%col.onCut(%client);
			return;
		}
		if (%col.getType() & $TypeMasks::PlayerObjectType && isObject(%client))
			return;
		Parent::onCollision(%projectile, %obj, %col, %fade, %pos, %normal);
	}
};
RPRegPackage(RPG_AxePackage);
