window.addEventListener('load', (e) => {
    document.getElementById('addProductToShoppingListForm').addEventListener('submit', async (e) => {
        handleAddProductToShoppingList(e);
    })
    document.getElementById("addProductToList").addEventListener("click", async (e) => {
        if (e.target.name != undefined) {
            console.log(e.target.name)
            addToShoppingList(e.target.name);
        }
    })
})

async function addToShoppingList(productID) {
    sessionStorage.setItem('selectedProductID', productID);

    $("#addProductToShoppingList").modal("show");

    let result = await fetch('/ShoppingList/ShoppingListDropDownList');
    let htmlResult = await result.text();
    document.getElementById("ddlContainer").innerHTML = htmlResult;
}

async function handleAddProductToShoppingList(e) {
    e.preventDefault();

    let productID = sessionStorage.getItem('selectedProductID');
    let listID = e.target["shoppingListDDL"].selectedOptions[0].value

    if (listID == 0) {
        return;
    }

    let shopListItem =
    {
        ShoppingListId: listID,
        ProductId: productID,
        Quantity: e.target["productQuantity"].value
    }

    console.log(shopListItem);

    let result = await fetch("/ShoppingList/AddShoppingListItem", {
        method: "POST",
        headers: {
            "content-type": "application/json"
        },
        body: JSON.stringify(shopListItem)
    });

    if (result.ok) {
        $("#addProductToShoppingList").modal("hide");
    }
    else if (!result.ok) {
        $("#addProductToShoppingList").modal("hide");
        $("#addProductDupItem").modal("show");
    }
}