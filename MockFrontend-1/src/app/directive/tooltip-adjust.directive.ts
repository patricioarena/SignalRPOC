import { Directive, ElementRef, Renderer2, HostListener, Input, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appTooltipAdjust]'
})
export class TooltipAdjustDirective implements AfterViewInit{
  @Input() appTooltipAdjust: boolean;

  private readonly MIN_CARD_WIDTH = '200px';
  private readonly MAX_CARD_WIDTH = '300px';
  private readonly MAX_CONTENT_WIDTH = '150px';
  private readonly FONT_SIZE = '14px';

  constructor(private element: ElementRef, private renderer: Renderer2) { }

  ngAfterViewInit(): void {
    console.log("ENTRO EN NGAFTERVIEWINIT")
    if(this.appTooltipAdjust){
      this.adjustTooltipPosition();
      this.adjustCardDimensions();
    }
  }

  // @HostListener('window:resize')
  // onWindowResize(){
  //   console.log("ONWINDOWRESIZE")
  //   this.adjustTooltipPosition();
  //   this.adjustCardDimensions();
  // }

  adjustTooltipPosition() {
    const element = this.element.nativeElement;
    const rect = element.getBoundingClientRect();
    const viewportWidth = window.innerWidth;
    const viewportHeight = window.innerHeight;

    // Obtener el ancho y alto del elemento
    const elementWidth = element.offsetWidth;
    const elementHeight = element.offsetHeight;

    // Obtener el contenedor padre para calcular la posición relativa
    const parentElement = element.parentElement;
    const parentRect = parentElement ? parentElement.getBoundingClientRect() : { left: 0, top: 0 };

    // Calcular la posición del tooltip en relación con el contenedor padre
    const offsetLeft = rect.left - parentRect.left;

    // Ajustar la posición si la tarjeta se sale del margen derecho
    if (rect.right > viewportWidth) {
      const overflowRight = rect.right - viewportWidth;
      const newLeft = Math.max(0, offsetLeft - overflowRight);
      this.setElementStyle('left', `${newLeft}px`);
    }

    // Ajustar la posición si la tarjeta se sale del margen izquierdo
    if (rect.left < 0) {
      const overflowLeft = Math.abs(rect.left);
      const newLeft = Math.max(20, offsetLeft + overflowLeft);
      this.setElementStyle('left', `${newLeft}px`);
    }
  }

  private adjustCardDimensions(): void {
    this.setElementStyle('max-width', this.MAX_CARD_WIDTH);
    this.setElementStyle('min-width', this.MIN_CARD_WIDTH);

    const cardBody = this.element.nativeElement.querySelector('.viewer-card-body');
    if (cardBody) {
      this.setElementStyle('max-width', this.MAX_CONTENT_WIDTH, cardBody);

      const textElements = cardBody.querySelectorAll('*');
      textElements.forEach((textElement: HTMLElement) => {
        this.setElementStyle('max-width', this.MAX_CONTENT_WIDTH, textElement);
        this.setElementStyle('font-size', this.FONT_SIZE, textElement);
      });
    }
  }

  private setElementStyle(style: string, value: string, element: HTMLElement = this.element.nativeElement): void {
    this.renderer.setStyle(element, style, value);
  }
}


