let ingredients = document.querySelector("#ingredients");//.querySelector('tbody'); 
let addIngredient = document.querySelector("#addIngredient");

addIngredient.addEventListener('click', AddIngredient());
ingredients.addEventListener('click', e => RemoveIngredient(e));

function AddIngredient() {
    let rowNo = ingredients.rows.length;

    let row = ingredients.insertRow(rowNo);
    let col1 = row.insertCell(0);
    let col2 = row.insertCell(1);
    let col3 = row.insertCell(2);

    let name = document.createElement('Input');
    name.name = "Ingredients[' + rowNo + '].Name";
    name.id = "Ingredients_' + rowNo + '_Name";

    let quantity = document.createElement('Input');
    quantity.name = "Ingredients[' + rowNo + '].Quantity";
    quantity.id = "Ingredients_' + rowNo + '_Quantity";

    let unit = document.createElement('Input');
    unit.name = "Ingredients[' + rowNo + '].Unit";
    unit.id = "Ingredients_' + rowNo + '_Unit";

    col1.append(name);
    col2.append(quantity);
    col3.append(unit);
}

function RemoveIngredient(e) {
    if (e.target.classList.contains('remove')) {
        e.target.closest('tr').remove();
    }
}