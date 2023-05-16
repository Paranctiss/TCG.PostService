using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.Gradings.DTO.Request;
using TCG.PostService.Application.Gradings.DTO.Response;


namespace TCG.PostService.Application.Gradings.Query
{

    public record GetGradingQuery() : IRequest<IEnumerable<GradingDtoResponse>>;

    public class GetGradingQueryHandler : IRequestHandler<GetGradingQuery, IEnumerable<GradingDtoResponse>>
    {
        private readonly ILogger<GetGradingQueryHandler> _logger;
        private readonly IGradingRepository _repository;
        private readonly IMapper _mapper;

        public GetGradingQueryHandler(ILogger<GetGradingQueryHandler> logger, IGradingRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GradingDtoResponse>> Handle(GetGradingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var grading = await _repository.GetAllGradingAsync(cancellationToken);

                if (grading == null)
                {
                    _logger.LogWarning("No grading found");
                    return null;
                }

                var gradingDto = _mapper.Map<IEnumerable<GradingDtoResponse>>(grading);

                return gradingDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving grading : {ErrorMessage}", ex.Message);
                throw;
            }
        }

    }
}
