function createStarrySky() {
    const style = document.createElement('style');
    style.textContent = `
    .star {
      position: absolute;
      top: 50%;
      left: 50%;
      height: 4px;
      background: linear-gradient(-45deg, rgb(255, 255, 255), rgba(0, 0, 255, 0));
      border-radius: 999px;
      filter: drop-shadow(0 0 6px rgba(255, 255, 255, 0.1)) drop-shadow(0 0 8px rgba(255, 255, 255, 0.1)) drop-shadow(0 0 20px rgba(255, 255, 255, 1));
      animation: animate 3s linear infinite;
    }
    
.star::before{
	content: '';
	position: absolute;
	top: calc(50% - 1px);
	right: 0;
	height: 2px;
	background: linear-gradient(-45deg, rgba(0, 0, 255, 0), rgb(255, 255, 255), rgba(0, 0, 255, 0));
	transform: translateX(50%) rotateZ(45deg);
	border-radius: 100%;
	animation: shining 3s ease-in-out infinite;
}

    .star::after{
	content: '';
	position: absolute;
	top: calc(50% - 1px);
	right: 0;
	height: 2px;
	background: linear-gradient(-45deg, rgba(0, 0, 255, 0), rgb(255, 255, 255), rgba(0, 0, 255, 0));
	border-radius: 100%;
	animation: shining 3s ease-in-out infinite;
	transform: translateX(50%) rotateZ(-45deg);
}

.star:nth-child(1) {
	top: calc(50% - -173px);
	left: 0;
    animation-delay:0 ;
}
    span:nth-child(1)::before, span:nth-child(1)::after {
	// animation-delay: 3358ms;
    animation-delay:0 ;
}

@keyframes shining {
    0% { width: 0; }
	50% { width: 50px; }
	100% { width: 0; }
}

@keyframes animate {
    0%
    {
		width: 0px;
        transform: rotate(45deg) translateX(0);
        opacity: 0.4;
    }
    50%
    {
		width: 100px;
        opacity: 1;
    }
    100%
    {
		width: 0px;
        transform: rotate(45deg) translateX(70vw);/* Giảm khoảng cách di chuyển để phù hợp màn hình */
        opacity: 0;
    }
}
  `;

    // Thêm thẻ <style> vào <head>
    document.head.appendChild(style);

    for (var i = 0; i < 20; i++) {
        const star = document.createElement('div');
        star.classList.add('star');

        // Vị trí ngẫu nhiên
        star.style.left = `${Math.random() * 100}vw`;
        star.style.top = `${Math.random() * -100}px`; // Bắt đầu từ trên cùng
        star.style.animationDelay = `${Math.random() * 3}s`; // Độ trễ ngẫu nhiên

        document.body.appendChild(star);
    }
}

createStarrySky();