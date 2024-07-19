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
  private readonly hideDelay = 5000;

  onThumbnailMouseEnter(usuario: Usuario): void {
    this.clearHideTimeout(); // Limpiar cualquier timeout anterior
    this.hoveredViewer = usuario;
  }

  onThumbnailMouseLeave(): void {
    this.scheduleHideTooltip();
  }

  onTooltipMouseEnter(): void {
    this.clearHideTimeout(); // Detener el ocultamiento cuando el mouse entra en la tarjeta
  }

  onTooltipMouseLeave(): void {
    this.scheduleHideTooltip();  // Programar el ocultamiento cuando el mouse sale de la tarjeta
  }

  private scheduleHideTooltip(): void {
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

}
