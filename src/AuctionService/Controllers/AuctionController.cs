

using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controller;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AuctionDbContext _auctionDbContext;

    public AuctionController(AuctionDbContext auctionDbContext, IMapper mapper)
    {
        _auctionDbContext = auctionDbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()

    {
        var auctions = await _auctionDbContext.Auction.Include(x => x.Item).OrderBy(x => x.Item.Make).ToListAsync();
        return _mapper.Map<List<AuctionDto>>(auctions);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _auctionDbContext.Auction.Include(_x => _x.Item).FirstOrDefaultAsync();
        if (auction == null) return NotFound();

        return _mapper.Map<AuctionDto>(auction);

    }
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAcuctionDto createAcuctionDto)
    {
        var auctionDto = _mapper.Map<Auction>(createAcuctionDto);

        auctionDto.Seller = "Test";
        _auctionDbContext.Auction.Add(auctionDto);
        var result = await _auctionDbContext.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could mot save changes to the Db");
        return CreatedAtAction(nameof(GetAuctionById),
          new { auctionDto.Id }, _mapper.Map<AuctionDto>(auctionDto));
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _auctionDbContext.Auction.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return NotFound();

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;

        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;

        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        var result = await _auctionDbContext.SaveChangesAsync() > 0;
        if (result) return Ok();
        return BadRequest("Problem for saving");

    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var auction = await _auctionDbContext.Auction.FindAsync(id);
        if (auction == null) return NotFound();

        _auctionDbContext.Auction.Remove(auction);

        var result = await _auctionDbContext.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not update Db");
        return Ok();
         

    }
    }