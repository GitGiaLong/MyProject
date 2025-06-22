function createStarrySky() {
    // Tạo container sky nếu chưa tồn tại
    let sky = document.querySelector('.sky');
    if (!sky) {
        sky = document.createElement('div');
        sky.classList.add('sky');
        document.body.appendChild(sky);
    }

    // Tạo 10 sao
    for (let i = 0; i < 10; i++) {
        const star = document.createElement('div');
        star.classList.add('star');
        star.style.left = `${Math.random() * 100}vw`;
        star.style.top = `${Math.random() * 100}vh`;
        star.style.animationDelay = `${Math.random() * 3}s`;
        sky.appendChild(star);
    }

    // Tạo con tàu phi hành gia
    const spaceship = document.createElement('div');
    spaceship.classList.add('spaceship');
    spaceship.id = ('spaceship');
    sky.appendChild(spaceship);

    spaceship.addEventListener('mouseover', function () {
        const positions = ['shift-left', 'shift-top', 'shift-right', 'shift-bottom'];
        const currentPosition = positions.find(dir => spaceship.classList.contains(dir));
        const nextPosition = positions[(positions.indexOf(currentPosition) + 1) % positions.length];
        if (nextPosition === positions[0])// shift-left
            spaceship.style.transform = `translateX(-${Math.floor(Math.random() * 200)}px) rotateZ(45deg)`;
        else if (nextPosition === positions[1])// shift-top
            spaceship.style.transform = `translateY(-${Math.floor(Math.random() * 200)}px) rotateZ(45deg)`;
        else if (nextPosition === positions[2])// shift-right
            spaceship.style.transform = `translateX(${Math.floor(Math.random() * 200)}px) rotateZ(45deg)`;
        else if (nextPosition === positions[3])// shift-bottom
            spaceship.style.transform = `translateY(${Math.floor(Math.random() * 200)}px) rotateZ(45deg)`;

        spaceship.classList.remove(currentPosition);
        spaceship.classList.add(nextPosition);
    });

}

// Đảm bảo DOM đã tải trước khi gọi hàm
document.addEventListener('DOMContentLoaded', createStarrySky);
