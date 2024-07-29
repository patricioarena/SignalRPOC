import { Component, OnInit } from '@angular/core';
import { SignalRService } from './signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {

  title = 'SignalRPOC';
  username = "Alice";
  connectionId = "";

  constructor(private signalRService: SignalRService) { }

  public ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.connectionId$.subscribe((id: string) => {
        this.connectionId = id;
    })
  }

}
