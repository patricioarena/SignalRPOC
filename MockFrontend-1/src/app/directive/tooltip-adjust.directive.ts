import { Directive, ElementRef, Renderer2, HostListener, Input, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appTooltipAdjust]'
})
export class TooltipAdjustDirective implements AfterViewInit{
  @Input() appTooltipAdjust: boolean;

  constructor(private element: ElementRef, private renderer: Renderer2) { }

  ngAfterViewInit(): void {
    console.log("ENTRO EN NGAFTERVIEWINIT")
    if(this.appTooltipAdjust){
      this.adjustTooltipPosition();
      this.setCardMaxWidth();
      this.setCardContentWidth();
    }
  }

  @HostListener('window:resize')
  onWindowResize(){
    console.log("ONWINDOWRESIZE")
    this.adjustTooltipPosition();
    // this.setCardMaxWidth();
    // this.setCardContentWidth();
  }

  adjustTooltipPosition() {
    const element = this.element.nativeElement;
    const rect = element.getBoundingClientRect();
    const viewportWidth = window.innerWidth;

    // Ajustar la posición si la tarjeta se sale del margen derecho
    if (rect.right > viewportWidth) {
      const overflowRight = rect.right - viewportWidth;
      this.renderer.setStyle(element, 'left', `-${overflowRight}px`);
    }

    // Ajustar la posición si la tarjeta se sale del margen izquierdo
    if (rect.left < 0) {
      const overflowLeft = Math.abs(rect.left);
      this.renderer.setStyle(element, 'left', `${overflowLeft}px`);
    }
  }


  private setCardMaxWidth(): void {
    const element = this.element.nativeElement;
    this.renderer.setStyle(element, 'max-width', '50px');
  }

  private setCardContentWidth(): void {
    const element = this.element.nativeElement;
    const textElements = element.querySelectorAll('.viewer-card-body, .viewer-card-body *');
    textElements.forEach((textElement: HTMLElement) => {
      this.renderer.setStyle(textElement, 'max-width', '50px');
    });
  }
}


