// --------------------------
// RP RealEstate :: Game
// --------------------------

if (isPackage(RP_RealEstate_Game_Boot))
	deactivatePackage(RP_RealEstate_Game_Boot);

package RP_RealEstate_Game_Boot
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
		
		RPDB.addValue("lotData", "0");
		RPDB.saveDefs();

		RP_AddStat("LotData", "lotData", 0);
		
		CRPG_AssembleEvents();
	}
};
activatePackage(RP_RealEstate_Game_Boot);

if (isPackage(RP_RealEstate_Game))
	deactivatePackage(RP_RealEstate_Game);

function gameConnection::getSalary(%client)
{
	return mFloor($RP::job::pay[%client.RPData.value["jobID"]]);
}

function gameConnection::sellZone(%client, %sellerID, %brick, %price)
{
	if(isObject(%brick) && %brick.getClassName() $= "fxDTSBrick" && %client.RPData.value["money"] >= %price)
	{
		if(%client.brickGroup.lotsOwned < $CRPG::pref::realestate::maxLots)
		{
			%brick.setDatablock(%brick.getDatablock().CRPGMatchingLot);
			%client.brickGroup.add(%brick);
			%brick.handleCRPGBrickCreation();
			
			messageClient(%client, '', '\c6You have purchased a \c3%1\c6 for $%2', %brick.getDatablock().uiName, %price);
			%client.RPData.value["money"] -= %price;
			
			if(%price)
			{
				if(isObject(%seller = FindClientByBL_ID(%sellerID)))
				{
					messageClient(%seller, '', '\c6You just gained \c3$%1\c6 for selling a zone to \c3%2\c6.', %price, %client.name);
					RPDB.set(%sellerID, "bank", RPDB.get(%OwnerID, "bank") + %price);
					CRPGData.getData(%sellerID).valueBank += %price;
				}
			}	
			
			%client.displayInfo();
		}
		else
			messageClient(%client, '', "\c6You already own enough lots.");
	}
}
