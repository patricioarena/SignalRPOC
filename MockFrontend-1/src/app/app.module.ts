import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ViewersCardComponent } from './viewers-card/viewers-card.component';
import { TooltipAdjustDirective } from './directive/tooltip-adjust.directive';

@NgModule({
  declarations: [
    AppComponent,
    ViewersCardComponent,
    TooltipAdjustDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
