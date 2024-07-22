import { Component, Input, OnInit } from '@angular/core';
import { Usuario } from '../Usuario';

@Component({
  selector: 'app-viewers-card',
  templateUrl: './viewers-card.component.html',
  styleUrls: ['./viewers-card.component.scss']
})
export class ViewersCardComponent implements OnInit{

  ngOnInit(): void {
  }

  @Input() usuarios : Usuario[];
  hoveredViewer: Usuario | null = null;
  private hideTimeout: any;
  private showTimeout: any;
  private readonly hideDelay = 700;
  private readonly showDelay = 300;

  onMouseEnter(usuario: Usuario, isTooltip: boolean = false): void {
    this.clearHideTimeout();
    this.clearShowTimeout();
    this.showTimeout = setTimeout(() => {
      this.hoveredViewer = usuario;
    }, this.showDelay);
  }

  onMouseLeave(): void {
    this.clearShowTimeout();
    this.scheduleHideTooltip();
  }

  private scheduleHideTooltip(): void {
    this.clearHideTimeout();
    this.hideTimeout = setTimeout(() => {
      this.hoveredViewer = null;
    }, this.hideDelay);
  }

  private clearHideTimeout(): void {
    if (this.hideTimeout) {
      clearTimeout(this.hideTimeout);
      this.hideTimeout = null;
    }
  }

    private clearShowTimeout(): void {
    if (this.showTimeout) {
      clearTimeout(this.showTimeout);
      this.showTimeout = null;
    }
  }

  // onThumbnailMouseEnter(usuario: Usuario): void {
  //   this.clearHideTimeout(); // Limpiar cualquier timeout anterior
  //   this.hoveredViewer = usuario;
  //   console.log("thumbnail", usuario)
  // }

  // onThumbnailMouseLeave(): void {
  //   this.scheduleHideTooltip();
  // }

  // onTooltipMouseEnter(): void {
  //   this.clearHideTimeout(); // Detener el ocultamiento cuando el mouse entra en la tarjeta
  // }

  // private scheduleHideTooltip(): void {
  //   this.clearHideTimeout();
  //   this.hideTimeout = setTimeout(() => {
  //     this.hoveredViewer = null;
  //   }, this.hideDelay);
  // }

  // onTooltipMouseLeave(): void {
  //   this.scheduleHideTooltip();  // Programar el ocultamiento cuando el mouse sale de la tarjeta
  // }

  // private clearHideTimeout(): void {
  //   if (this.hideTimeout) {
  //     clearTimeout(this.hideTimeout);
  //     this.hideTimeout = null;
  //   }
  // }

}
