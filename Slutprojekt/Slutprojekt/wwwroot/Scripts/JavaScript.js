const buttons = document.querySelectorAll('button');
const carouselItems = document.querySelectorAll('.RecipeLoop-item');

buttons.forEach(button => button.addEventListener('click', (e) => {
    carouselItems.forEach(item => {
        item.classList.add('hide')
    })

    document.querySelector(`#RecipeLoop-item-${e.target.dataset.itemToShow}`).classList.remove('hide')


}))