// import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';

// @Injectable({
//   providedIn: 'root',
// })
// export class StyleAdjusterService {
//   private renderer: Renderer2;

//   constructor(rendererFactory: RendererFactory2) {
//     this.renderer = rendererFactory.createRenderer(null, null);
//   }

//   adjustStyles(container: HTMLElement): void {
//     this.renderer.setStyle(container, 'max-width', '300px');
//     this.renderer.setStyle(container, 'box-sizing', 'border-box');

//     const children = Array.from(container.querySelectorAll('.viewer-card-tooltip-container'));

//     children.forEach((child: HTMLElement) => {
//       this.renderer.setStyle(child, 'max-width', '300px');
//       this.renderer.setStyle(child, 'box-sizing', 'border-box');
//     });
//   }
// }
