let ingredients = document.querySelector("#ingredients_tbody"); 
let addIngredient = document.querySelector("#addIngredient");

addIngredient.addEventListener('click', e => AddIngredient(e));
ingredients.addEventListener('click', e => RemoveIngredient(e));

function AddIngredient(e) {
    if (e.target.id != "addIngredient") return;
    
    let rowNo = ingredients.rows.length;
    let row = ingredients.insertRow(rowNo);
    let col1 = row.insertCell(0);
    let col2 = row.insertCell(1);
    let col3 = row.insertCell(2);
    let col4 = row.insertCell(3);

    let name = document.createElement('Input');
    name.name = `Ingredients[${rowNo}].Name`;
    name.id = `Ingredients_${rowNo}_Name`;

    let quantity = document.createElement('Input');
    quantity.name = `Ingredients[${rowNo}].Quantity`;
    quantity.id = `Ingredients_${rowNo}_Quantity`;

    let unit = document.createElement('Input');
    unit.name = `Ingredients[${rowNo}].Unit`;
    unit.id = `Ingredients_${rowNo}_Unit`;

    let removeButton = document.createElement('a');
    removeButton.innerHTML = 'Remove';
    removeButton.href = "#";
    removeButton.classList.add('remove');

    col1.append(name);
    col2.append(quantity);
    col3.append(unit);
    col4.append(removeButton);
}

function RemoveIngredient(e) {
    if (e.target.classList.contains('remove')) {
        e.target.closest('tr').remove();
    }
}