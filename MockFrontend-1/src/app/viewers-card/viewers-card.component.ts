import { Component, Input, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { Usuario } from '../Usuario';
// import { StyleAdjusterService } from '../services/style-adjuster.service';

@Component({
  selector: 'app-viewers-card',
  templateUrl: './viewers-card.component.html',
  styleUrls: ['./viewers-card.component.scss']
})
export class ViewersCardComponent implements OnInit, AfterViewInit{
  @ViewChild('viewerCardContainer', { static: false }) viewerCardContainer: ElementRef;

  ngOnInit(): void {
  }

  @Input() usuarios : Usuario[];
  hoveredViewer: Usuario | null = null;
  private hideTimeout: any;
  private showTimeout: any;
  private readonly hideDelay = 700;
  private readonly showDelay = 300;
  private visibleUserCount = 10; // Número máximo de usuarios visibles inicialmente
//  private showAll = false; // Flag para mostrar todos los usuarios
  isDropdownOpen = false; //Controla la visibilidad del dropdown

  // private styleAdjuster: StyleAdjusterService
  constructor() {}

  ngAfterViewInit(){
    if(this.viewerCardContainer){
      // this.styleAdjuster.adjustStyles(this.viewerCardContainer.nativeElement)
    }
  }

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

  // Método para alternar la visibilidad de los usuarios
  toggleUsers() {
    // this.showAll = !this.showAll;
    // this.visibleUserCount = this.showAll ? this.usuarios.length : 10;
    this.isDropdownOpen = !this.isDropdownOpen; //Alterna el estado del dropdown
    console.log(this.isDropdownOpen)
  }

  // Obtener usuarios restantes
  get remainingUsers() {
    return this.usuarios.slice(this.visibleUserCount);
  }

  // Obtener usuarios visibles en función del estado
  get visibleUsers() {
    return this.usuarios.slice(0, this.visibleUserCount);
  }

  // Determinar si se debe mostrar el icono de "más"
  get showMoreIcon() {
    // return !this.showAll && this.usuarios.length > 10;
    return this.usuarios.length > this.visibleUserCount;
  }
}
