import { Component, Input, OnInit, AfterViewInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { Usuario } from '../Usuario';
// import { StyleAdjusterService } from '../services/style-adjuster.service';

@Component({
  selector: 'app-viewers-card',
  templateUrl: './viewers-card.component.html',
  styleUrls: ['./viewers-card.component.scss']
})
export class ViewersCardComponent implements OnInit, AfterViewInit{
  @ViewChild('viewerCardContainer', { static: false }) viewerCardContainer: ElementRef;
  @ViewChild('dropdownMenu', { static: false }) dropdownMenu: ElementRef; // Elemento del dropdown

  @Input() usuarios : Usuario[];
  hoveredViewer: Usuario | null = null;
  private hideTimeout: any;
  private showTimeout: any;
  private readonly hideDelay = 700;
  private readonly showDelay = 300;

  // Numero maximo de usuarios visibles inicialmente
  private visibleUserCount = 6;

  // Flag para mostrar todos los usuarios
  // private showAll = false;

  //Controla la visibilidad del dropdown
  isDropdownOpen = false;

  constructor(private eRef: ElementRef) {}

  ngOnInit(): void {
  }

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

  // Metodo para alternar la visibilidad de los usuarios del dropdown
  toggleUsers(): void {
    // this.showAll = !this.showAll;
    // this.visibleUserCount = this.showAll ? this.usuarios.length : 10;

    // Alterna el estado del dropdown
    this.isDropdownOpen = !this.isDropdownOpen;
    //console.log(this.isDropdownOpen)
  }

  // Obtener usuarios restantes en funcion del estado
  getRemainingUsers(): Array<any> {
    return this.usuarios.slice(this.visibleUserCount);
  }

  // Metodo para contar los usuarios restantes
  getRemainingUserCount(): number {
    return this.getRemainingUsers().length;
  }

  // Obtener usuarios visibles en funcion del estado
  getVisibleUsers(): Array<any> {
    return this.usuarios.slice(0, this.visibleUserCount);
  }

  // Determinar si se debe mostrar el icono de mas
  shouldshowMoreIcon(): boolean {
    // return !this.showAll && this.usuarios.length > 10;
    return this.usuarios.length > this.visibleUserCount;
  }

  // Detectar clic fuera del dropdown para cerrarlo
  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    if (this.isDropdownOpen && this.dropdownMenu && !this.eRef.nativeElement.contains(event.target)) {
      this.isDropdownOpen = false;
    }
  }
}
