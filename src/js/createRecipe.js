let ingredients = document.querySelector("#ingredients").querySelector('tbody'); 
let addIngredient = document.querySelector("#addIngredient");

ingredients.addEventListener('click', e => onClick(e));

function onClick(e) {
    let target = e.target;

    if (target.id == addIngredient.id) {
        AddIngredient();
        return;
    }

    if (target.classList.contains('remove')) {
        RemoveIngredient(e);
        return;
    }
}

function AddIngredient() {
    let rowNo = ingredients.rows.length;
    let template = 
        '<tr>' +
        '<td><input name="Ingredients[' + rowNo + '].Name" id="Ingredients_' + rowNo + '_Name" /></td>' +
        '<td><input name="Ingredients[' + rowNo + '].Quantity" id="Ingredients_' + rowNo + 'Quantity" /></td>' +
        '<td><input name="Ingredients[' + rowNo + '].Unit" id="Ingredients_' + rowNo + 'Unit" /></td>' +
        '</tr>';

    ingredients.append(template);
}

function RemoveIngredient(e) {
    e.target.closest('tr').remove();
}