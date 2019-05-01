const buttons = document.querySelectorAll('button');
const carouselItems = document.querySelectorAll('.carousel-item');

console.log(buttons);


console.log('hello from js file');


buttons.forEach(button => button.addEventListener('click', (e) => {
    carouselItems.forEach(item => {
        item.classList.add('hide')
    })

    console.log(e.target.dataset.itemToShow);
    document.querySelector(`#carousel-item-${e.target.dataset.itemToShow}`).classList.remove('hide')

    console.log(button);

}))