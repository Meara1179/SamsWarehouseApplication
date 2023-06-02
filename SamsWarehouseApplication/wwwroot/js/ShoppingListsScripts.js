// Adds an event listen that listens for when the page loads.
window.addEventListener("load", async (e) => {
    updateDDL();
    document.getElementById("createShoppingListForm").addEventListener('submit', async (e) => {
        await handleCreateShoppingList(e);
    })

    document.getElementById("newShoppingListButton").addEventListener("click", (e) => {
        $('#createShoppingList').modal('show')
    })

    document.getElementById("deleteShoppingList").addEventListener("click", (e) => {
        removeShoppingList()
    })

    document.getElementById("updateShoppingListItemQuantityForm").addEventListener("submit", (e) => {
        UpdateShoppingListItemQuantity(e.target["updateProductQuantity"].value)
    })
});

// Fetches the shopping list drop down list and replaces the indexes of the drop down list with the fetched data.
async function updateDDL() {
    let result = await advFetch('/ShoppingList/ShoppingListDropDownList');
    let htmlResult = await result.text();
    document.getElementById("ddlContainer").innerHTML = htmlResult;

    let ddlCOntainer = document.getElementById("ddlContainer");
    let ddl = ddlCOntainer.querySelector("select");
    ddl.addEventListener("change", async (e) => {
        handleDDLChange(e);
    })
}

// Activates on changing the selected list in the drop down list, saves the selected list ID and name to storage.
async function handleDDLChange(e) {
    let selectedOption = e.target.selectedOptions[0];
    sessionStorage.setItem("listID", selectedOption.value);
    sessionStorage.setItem("listName", selectedOption.text);

    RefreshItemList(selectedOption.value);
}

// Passes supplied value into fetch request to create new Shopping List, client side validation ensuring list name is between 1 and 200 characters.
async function handleCreateShoppingList(e) {
    e.preventDefault();

    let button = document.getElementById("btnAddShoppingList");

    button.setAttribute("disabled", "disabled");
    button.innerHTML =
        `
            <span class="spinner-border text-danger spinner-border-sm" role="status" aria-hidden="true"></span>
            Please Wait...
        `;

    if (e.target["listName"].value.length >= 1 && e.target["listName"].value.length <= 200) {
        let result = await advFetch("/ShoppingList/AddNewShoppingList", {
            method: "POST",
            headers: {
                "content-type": "application/json"
            },
            body: JSON.stringify(e.target["listName"].value)
        });
        if (result.ok) {
            await updateDDL();
            $("#createShoppingList").modal("hide");

            button.removeAttribute("disabled");
            button.innerHTML = "Create Shopping List"
            document.getElementById("createShoppingListName").value = "";
            document.getElementById("createShoppingListName").placeholder = "Please input Shopping List name...";
            sessionStorage.setItem("listID", 0);
        }
        if (result.status === 409) {
            document.getElementById("createShoppingListName").value = "";
            document.getElementById("createShoppingListName").placeholder = "Name already exists...";
            button.removeAttribute("disabled");
            button.innerHTML = "Create Shopping List"
        }
        else {
            document.getElementById("createShoppingListName").value = "";
            document.getElementById("createShoppingListName").placeholder = "Name must be between 1 and 200 characters long.";
            button.removeAttribute("disabled");
            button.innerHTML = "Create Shopping List"
        }
    }

}

// Deletes the currently selected shopping list in the drop down list.
async function removeShoppingList() {
    let listID = sessionStorage.getItem("listID");

    let result = await advFetch("ShoppingList/RemoveShoppingList?listID=" + listID, {
        method: 'DELETE'
    });

    if (result.ok) {
        updateDDL();
    }
}

// Removes the selected item from the specific shopping list.
async function removeShoppingListItem(itemID) {
    let listID = sessionStorage.getItem("listID");

    let shopListItem =
    {
        ShoppingListId: listID,
        ProductId: itemID
    }

    let result = await advFetch("ShoppingList/RemoveShoppingListItem", {
        method: 'DELETE',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(shopListItem)
    });

    if (result.ok) {
        RefreshItemList(listID);
        sessionStorage.setItem("listID", 0);
    }
}

// Handles displaying the shopping list items of the selected shopping list in the table.
async function RefreshItemList(listID) {
    let result = await fetch("/ShoppingList/GetShoppingListItems?listID=" + listID);
    let htmlResult = await result.text();
    document.getElementById("shoppingItemContainer").innerHTML = htmlResult;

    document.getElementById("shoppingListItemsList").removeEventListener("click",
        document.getElementById("shoppingListItemsList").addEventListener("click", (e) => {
            console.log(e.target.name)
            let buttonPressed = e.target.name;
            let id = buttonPressed.replace(/[a-zA-Z]+[^a-zA-Z0-9 ]/g, '');
            let command = buttonPressed.replace(/[^a-zA-Z]+/g, '');

            console.log(id);
            console.log(command);

            if (command == "DELETE") {
                removeShoppingListItem(id)
            }
            else if (command == "UPDATE") {
                SpawnUpdateQuantityModal(id)
            }
        })
    )
}

// Shows the update product quantity modal.
async function SpawnUpdateQuantityModal(itemID) {
    sessionStorage.setItem("productID", itemID);
    $('#updateShoppingListItemQuantity').modal('show')

}

// Takes the inputed value and updates the selected shopping list item with the new quantity.
async function UpdateShoppingListItemQuantity(quantity) {
    let listID = sessionStorage.getItem("listID");
    let itemID = sessionStorage.getItem("productID")

    console.log("PASS!")

    let shopListItem =
    {
        ShoppingListId: listID,
        ProductId: itemID,
        Quantity: quantity
    }

    let result = await advFetch("/ShoppingList/UpdateShoppingListItemQuantity", {
        method: "PUT",
        headers: {
            "content-type": "application/json"
        },
        body: JSON.stringify(shopListItem)
    });

    if (result.ok) {
        RefreshItemList(listID);
    }
}