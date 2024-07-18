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
  keepOpen: boolean = false;

  showViewerCard(usuario: Usuario): void {
    setTimeout(() => {
      this.hoveredViewer = usuario;
    }, 1000);
  }

  hideViewerCard(): void {
    if (!this.keepOpen){
      this.hoveredViewer = null;
    }
  }

  onViewerCardMouseEnter(): void {
    this.keepOpen = true;
  }

  onViewerCardMouseLeave(): void {
    this.keepOpen = false;
    this.hoveredViewer = null;
  }
}
