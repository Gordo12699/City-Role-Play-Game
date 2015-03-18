// --------------------------
// RP Money :: Events
// --------------------------

if (isPackage(RP_Money_Events))
	deactivatePackage(RP_Money_Events);

package RP_Money_Events
{
	// Picking up item from brick
	function ItemData::onPickup(%this, %item, %player)
	{
		Parent::onPickup(%this, %item, %player);
		
		%brick = %item.spawnBrick;
		
		if (isObject(%brick))
		{
			%brickGroup = getBrickGroupFromObject(%brick);
			%client = %brickGroup.client;
			if (!%client.isSuperAdmin)
				%brick.setItem(0, %player.client, true);
		}
	}
	
	// Pick up from ground
	function Weapon::onPickup(%this, %item, %player)
	{
		parent::onPickup(%this, %item, %player);
		
		%brick = %item.spawnBrick;
		
		if (isObject(%brick))
		{
			%brickGroup = getBrickGroupFromObject(%brick);
			%client = %brickGroup.client;
			if (!%client.isSuperAdmin)
				%brick.setItem(0, %player.client, true);
		}
	}
	
	// When setting an item ot a brick
	function fxDTSBrick::setItem(%brick, %datablock, %client, %a)
	{
		if (%brick == $LastLoadedBrick || !isObject(%client) || %client.isSuperAdmin || !%client.isInRP())
		{
			Parent::setItem(%brick, %datablock, %client, %a);
			return;
		}
		%lastItem = %brick.item;
		if (!isObject(%datablock) || (isObject(%lastItem) && %lastItem.getDatablock() == %datablock))
		{
			Parent::setItem(%brick, %datablock, %client, %a);
			return;
		}
		
		if (BuyItem(%client, %brick, %datablock))
			Parent::setItem(%brick, %datablock, %client, %a);
	}
	
	// Spawning the item from the brick
	function fxDTSBrick::spawnItem(%brick, %pos, %datablock, %client, %a)
	{
		if (%brick == $LastLoadedBrick || !isObject(%client) || %client.isSuperAdmin || !%client.isInRP())
		{
			Parent::spawnItem(%brick, %pos, %datablock, %client, %a);
			return;
		}
		
		%lastItem = %brick.item;
		if (!isObject(%datablock) || (isObject(%lastItem) && %lastItem.getDatablock() == %datablock))
		{
			Parent::spawnItem(%brick, %pos, %datablock, %client, %a);
			return;
		}
		
		if (BuyItem(%client, %brick, %datablock))
			Parent::spawnItem(%brick, %pos, %datablock, %client, %a);
	}
	
	// Adding the item to the player
	function Player::addItem(%player, %datablock, %client)
	{
		// The brick that was clicked
		%brick = $inputTarget_Self;
		if ((!isObject(%brick)) || %client.isSuperAdmin || (!%client.isInRP()))
			return;
		
		if (BuyItem(%client, %brick, %datablock))
			Parent::addItem(%player, %datablock, %client);
	}
	
	// Adding the item to the player
	function Player::setWeapon(%player, %datablock, %client)
	{
		// The brick that was clicked
		%brick = $inputTarget_Self;
		if ((!isObject(%brick)) || %client.isSuperAdmin || (!%client.isInRP()))
			return;
		
		if (BuyItem(%client, %brick, %datablock))
			Parent::setWeapon(%player, %datablock, %client);
	}
	function Player::setInventory(%player, %datablock, %slot, %client)
	{
		// The brick that was clicked
		%brick = $inputTarget_Self;
		if ((!isObject(%brick)) || %client.isSuperAdmin || (!%client.isInRP()))
			return;
		
		if (BuyItem(%client, %brick, %datablock))
			Parent::setWeasetInventorypon(%player, %datablock, %slot, %client);
	}
};
RPRegPackage(RP_Money_Events);

// Successfull transfering
function fxDTSBrick::onTransferSuccess(%brick, %client)
{
	$inputTarget_self = %brick;
	$inputTarget_client = %client;
	$inputTarget_player = %client.player;
	%brick.processInputEvent("onTransferSuccess", %client);
}

function fxDTSBrick::onRPTransferDecline(%brick, %client)
{
	$inputTarget_self	= %brick;
	$inputTarget_client	= %client;
	
	for(%i = 0; %i < %brick.numEvents; %i++)
	{
		if(%brick.eventInput[%i] $= "onRPTransferDecline" && (%brick.eventOutput[%i] $= "requestFunds" || %brick.eventOutput[%i] $= "sellItem" || %brick.eventOutput[%i] $= "sellFood"))
			%brick.eventEnabled[%i] = false;
	}
	
	%brick.processInputEvent("onRPTransferDecline", %client);
}

// Transfering money to brick
//function fxDTSBrick::transferMoneyToBrick(%brick, %service, %amount, %client)
//{
//	if (!%client.isInRP())
//		return;
	
//	%amount = atoi(%amount);
//	if (%amount <= 0)
//	{
//		messageClient(%client, '', '\c6Invalid amount of %1.', $RP::pref::moneyName);
//		return;
//	}
	
//	if (%client.RPData.value["money"] < %amount)
//	{
//		messageClient(%client, '', '\c6You do not have enough %1 to pay for this service.', $RP::pref::moneyName);
	//	return;
//	}
	
//	%brickGroup = getBrickGroupFromObject(%brick);
	//%target = %brickGroup.client;
	// Owner only
	//if (%target == %client)
//	{
//		// Transfer credits
//		%client.RPData.value["money"] -= %amount;
//		%brick.RP_money += %amount;
//		messageClient(%client, '', '\c6You transferred \c3%1\c6 to your brick. (It is not recommended to store money in a brick.)', prepareDisplayMoney(%amount));
//		%brick.onTransferSuccess(%client);
	//	return;
//	}
	
	//if (RequestTransfer(%client, %brick, %service, %amount))
//	{
//		%client.activateBrick = %brick;
//		%client.activateTransfer = %service;
//		%client.cost = %amount;
//	}
//}

// Check current money
//function fxDTSBrick::displayCurrentMoney(%brick, %client)
//{
//	if (!%client.isInRP())
//		return;
	
//	%amount = atoi(%brick.RP_money);
//	messageClient(%client, '', '\c6This brick has \c3%1\c6 available.', prepareDisplayMoney(%amount));
//}

// Transfer money to client
//function fxDTSBrick::transferMoneyToClient(%brick, %trust, %client)
//{
//	if (!%client.isInRP())
//		return;
//	if (!isObject(%client))
//		return;
//	if (getTrustLevel(%client, %brick) < %trust)
//	{
//		messageClient(%client, '', "\c6You do not have enough trust to use this event.");
//		return;
//	}
	
//	%amount = atoi(%brick.RP_money);
	
//	if (%amount <= 0)
//	{
//		messageClient(%client, '', '\c6There is currently no %1 in brick.', $RP::pref::moneyName);
//		return;
//	}
	
//	%client.RPData.value["money"] += %amount;
//	%brick.RP_money = 0;
//	messageClient(%client, '', '\c6You transferred \c3%1\c6 from the brick to your wallet.', prepareDisplayMoney(%amount));
//	%client.displayInfo();
//}

function fxDTSBrick::RPsellItem(%brick, %item, %markup, %client)
{
	if(isObject(%client.player) && !%client.player.serviceOrigin  && isObject(%brick))
	{
		%name = $CRPG::prices::weapon::name[%item].uiName;
		
		if($CRPG::citydata::minerals >= $CRPG::prices::weapon::mineral[%item])
		{
			%client.player.serviceType = "item";
			%client.player.serviceItem = %item;
			%client.player.serviceFee = $CRPG::prices::weapon::price[%item] + %markup;
			%client.player.serviceMarkup = %markup;
			%client.player.serviceOrigin = %brick;
			
			messageClient(%client,'',"\c6A service is offering to sell you one \c3" @ %name SPC "\c6for \c3$" @ %client.player.serviceFee SPC "\c6.");
			messageClient(%client,'',"\c6Accept with \c3/yes\c6, decline with \c3/no\c6.");
		}
		else
			messageClient(%client, '', '\c6A service is trying to offer you a(n) \c3%1\c6, but the city needs \c3%2\c6 more minerals to produce it!', %name, ($CRPG::prices::weapon::mineral[%item] - $CRPG::citydata::minerals));
	}
	else if(%client.player.serviceOrigin && %client.player.serviceOrigin != %brick)
		messageClient(%client, '', "\c6You already have a charge request from another service! Type \c3/no\c6 to reject it.");
}

function gameConnection::RPsellItem(%client, %sellerID, %itemID, %price, %profit)
{
	if(isObject(%client.player) && %client.RPData.value["money"] >= %price)
	{
		if($RP::job::sellItems[RPDB.get(%SellerID, "jobID")] || RPDB.get(%SellerID, "shopLicense"))
		{							
			for(%a = 0; %a < %client.player.getDatablock().maxTools; %a++)
			{
				if(!isObject(%obj.tool[%a]) || %obj.tool[%a].getName() !$= $CRPG::prices::weapon::name[%itemID])
				{
					if(%freeSpot $= "" && %client.player.tool[%a] $= "") { %freeSpot = %a; }
				}
				else
				{
					%alreadyOwns = true;
				}
			}
			
			if(%freeSpot !$= "" && !%alreadyOwns)
			{
				%client.RPData.value["money"] -= %price;
				findclientbyBL_ID(%sellerID).RPData.value["bank"] += %profit;
				$CRPG::citydata::minerals -= $CRPG::prices::weapon::mineral[%itemID];
		
				%client.player.tool[%freeSpot] = $CRPG::prices::weapon::name[%itemID].getID();
				messageClient(%client, 'MsgItemPickup', "", %freeSpot, %client.player.tool[%freeSpot]);
				
				messageClient(%client, '', "\c6You have accepted the item's fee of \c3$" @ %price @ "\c6!");
				%client.updateInfo();
				
				if(findclientbyBL_ID(%sellerID))
				{
					messageClient(findclientbyBL_ID(%sellerID), '', '\c6You gained \c3$%1\c6 selling \c3%2\c6 an item.', %profit, %client.name);
					findclientbyBL_ID(%sellerID).RPData.value["BusinessExp"] += 1;
				}
				
			//%client.player.serviceOrigin.onTransferSuccess(%client);
			}
			else if(%alreadyOwns)
				messageClient(%client, '', "\c6You don't need another\c3" SPC $CRPG::prices::weapon::name[%itemID].uiName @ "\c6.");
			else if(%freeSpot $= "")
				messageClient(%client, '', "\c6You don't have enough space to carry this item!");
		}
		else
		{
			messageClient(%client, '' , "\c6This vendor is not licensed to sell items.");
		}
	}
}
