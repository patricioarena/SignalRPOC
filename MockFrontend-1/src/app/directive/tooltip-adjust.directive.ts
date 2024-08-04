import { Directive, ElementRef, Renderer2, Input, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appTooltipAdjust]'
})
export class TooltipAdjustDirective implements AfterViewInit{
  @Input() appTooltipAdjust: boolean;

  private readonly MIN_CARD_WIDTH = '200px';
  private readonly MAX_CARD_WIDTH = '250px';
  private readonly MAX_CONTENT_WIDTH = '150px';
 // private readonly FONT_SIZE = '15px';

  constructor(private element: ElementRef, private renderer: Renderer2) { }

  ngAfterViewInit(): void {
    // console.log("ENTRO EN NGAFTERVIEWINIT")
    if(this.appTooltipAdjust){
      this.adjustTooltipPosition();
      this.adjustCardDimensions();
    }
  }

  adjustTooltipPosition() {
    const element = this.element.nativeElement;
    const rect = element.getBoundingClientRect();

    // Obtener el contenedor .viewers-container y su rectángulo delimitador
    const viewersContainer = document.querySelector('.viewer-thumbnail-wrapper');
    if (!viewersContainer) {
      console.error('Contenedor no encontrado');
      return;
    }
    const containerRect = viewersContainer.getBoundingClientRect();

    // Obtener el elemento padre (asumiendo que es la imagen)
    const parentElement = element.parentElement;
    const parentRect = parentElement.getBoundingClientRect();

    // Calcular la posicion izquierda del tooltip en relacion con el contenedor
    let offsetLeft = parentRect.left - containerRect.left;

    // Ajustar la posicion si la tarjeta se sale del margen derecho del contenedor
    if (rect.right > containerRect.right) {
      // console.log("margen derecho")
      const overflowRight = rect.right - containerRect.right;
      offsetLeft = Math.max(-145, offsetLeft - overflowRight);
      this.setElementStyle('left', `${offsetLeft}px`);
    }

    // Ajustar la posicion si la tarjeta se sale del margen izquierdo del contenedor
    if (rect.left < containerRect.left) {
      // console.log("margen izquierdo")
      const overflowLeft = containerRect.left - rect.left;
      offsetLeft = Math.max(-100, offsetLeft + overflowLeft);
      this.setElementStyle('left', `${offsetLeft}px`);
    }

    // Aplicar los estilos de posicion al tooltip
    this.setElementStyle('left', `${offsetLeft}px`);

  }

  private adjustCardDimensions(): void {
    // console.log("ajusto dimension card")

    // Establece el ancho maximo y minimo de la tarjeta principal
    this.setElementStyle('max-width', this.MAX_CARD_WIDTH);
    this.setElementStyle('min-width', this.MIN_CARD_WIDTH);

    // Obtiene el elemento del cuerpo de la tarjeta .viewer-card-body
    const cardBody = this.element.nativeElement.querySelector('.viewer-card-body');
    if (cardBody) {
      // Establece el ancho maximo del contenido dentro del cuerpo de la tarjeta
      this.setElementStyle('max-width', this.MAX_CONTENT_WIDTH, cardBody);

      // Selecciona todos los elementos de texto dentro del cuerpo de la tarjeta
      const textElements = cardBody.querySelectorAll('*');

      textElements.forEach((textElement: HTMLElement) => {
        // Establece el ancho maximo del contenido para cada elemento de texto
        this.setElementStyle('max-width', this.MAX_CONTENT_WIDTH, textElement);

        // this.setElementStyle('font-size', this.FONT_SIZE, textElement);
      });
    }
  }

  // Metodo para establecer el estilo a un elemento
  private setElementStyle(style: string, value: string, element: HTMLElement = this.element.nativeElement): void {
    this.renderer.setStyle(element, style, value);
  }
}


