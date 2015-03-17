// --------------------------
// RP Hunger :: User
// --------------------------

if (isPackage(RP_Hunger_User_Core))
	deactivatePackage(RP_Hunger_User_Core);

package RP_Hunger_User_Core
{
	function player::setScale(%this, %scale, %client)
	{
		parent::setScale(%this, %scale);
		
		if(%client.RPData.value["hunger"] > 9)
			%scale = "1.100 1.100 1";
		else if(%client.RPData.value["hunger"] == 1)
			%scale = "0.900 0.900 1";
	}
};
activatePackage(RP_Hunger_User_Core);
