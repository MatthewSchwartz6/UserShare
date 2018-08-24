import { TestBed, inject } from '@angular/core/testing';

import { PostcommentsService } from './postcomments.service';

describe('PostcommentsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PostcommentsService]
    });
  });

  it('should be created', inject([PostcommentsService], (service: PostcommentsService) => {
    expect(service).toBeTruthy();
  }));
});
