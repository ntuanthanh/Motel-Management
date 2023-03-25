function zoomImages() {
    const images = document.querySelectorAll('.gallery_cus img');
    const overlay = document.createElement('div');
    console.log(images.length);

    function showImage(e) {
        overlay.classList.add('overlay_cus');
        overlay.innerHTML = `<img src="${this.src}" alt="${this.alt}">
                                                    <div class="close">&times;</div>`;
        document.body.appendChild(overlay);
    }

    function hideImage() {
        overlay.innerHTML = '';
        overlay.classList.remove("overlay_cus");
    }

    images.forEach(image => {
        image.addEventListener('click', showImage);
    });

    overlay.addEventListener('click', hideImage);
}