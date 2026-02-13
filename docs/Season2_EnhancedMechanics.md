# Crimson Compass Season 2: Enhanced Supply Chain Investigation Mechanics

## Overview
Season 2 "Supply Chain Ghosts" has been enhanced with adapted versions of advanced mechanics, rethemed for logistics and supply chain investigation scenarios. These mechanics build upon Season 1's corporate tools while introducing supply chain complexity.

## Adapted Core Mechanics

### Paradox System (Supply Chain Edition)
**Purpose:** Creates uncertainty in logistics operations and shipping anomalies.

**Implementation:**
- `paradoxLevel`: Scales from 1-3 through Season 2 episodes
- `paradoxEvent`: Supply chain contradictions requiring logistics mapping
- Moderate escalation building toward Season 3

**Gameplay Impact:**
- Supply chain paradoxes (e.g., shipments that exist but can't be tracked)
- Logistics contradictions create investigation tension
- Prepares players for Season 3's recovery paradoxes

### Chain Scanners
**Purpose:** Detects vulnerabilities and manipulations in supply chain operations.

**Gadget Details:**
- **Name:** CHAINSCANNER
- **Function:** Reveals supply chain weaknesses and diversion points
- **Usage:** Activated on clues with `chainScanner` field

**Scanner Properties:**
- `detects`: Type of vulnerability (supply-chain-weakness, diversion-point, etc.)
- `difficulty`: Scanning challenge level (medium for Season 2)
- Success reveals logistics manipulation evidence

### Logistics Analyzers
**Purpose:** Analyzes delivery timing and supply chain scheduling anomalies.

**Gadget Details:**
- **Name:** LOGISTICSANALYZER
- **Function:** Identifies timing disruptions in supply chain operations
- **Usage:** Puzzle mechanic for delivery-based clues

**Analyzer Properties:**
- `reveals`: Information type (delivery-timing-anomaly, route-disruption)
- `puzzleType`: Interaction style (route-optimization, timing-calculation)
- Builds on Season 1's log analyzers with supply chain focus

### Jurisdictional Puzzles (Supply Chain Edition)
**Purpose:** Resolves conflicts between customs authorities and commercial shipping entities.

**Implementation:**
- `jurisdictionConflicts`: Customs vs commercial authority disputes
- `jurisdictionPuzzle`: Authority resolution mechanic

**Puzzle Properties:**
- `conflictType`: Type of dispute (customs-vs-commercial, shipping-vs-border)
- `authorities`: Involved parties (Customs Authority, Shipping Company, International Trade)
- `solution`: Required resolution method (Customs-clearance, Trade-agreement)

**Gameplay Impact:**
- Introduces international shipping complexity
- Customs and border authority considerations
- Escalates jurisdictional puzzles from Season 1

### Supply Network Interfaces
**Purpose:** Maps global supply chain connections and logistics routes.

**Gadget Details:**
- **Name:** SUPPLYNETWORK
- **Function:** Visualizes supply chain relationships and diversion routes
- **Usage:** Strategic overview tool for logistics investigations

**Network Properties:**
- `networkConnections`: Links between supply chain nodes and cases
- `connectionType`: Logistics relationship (logistics-route, diversion-path)
- `paradoxRisk`: Medium risk for Season 2

**Additional Tools:**
- Enhanced **JAMMER**: Supply chain signal disruption
- **CLEANER**: Logistics evidence sanitization

## Enhanced Clue System

### Multi-Modal Clues (Season 2)
Season 2 clues support multiple logistics investigation types:

```json
{
  "id": "C1",
  "text": "Evidence related to The Logistics Coordinator",
  "leadsTo": ["S1", "M1"],
  "chainScanner": {
    "detects": "supply-chain-vulnerability",
    "difficulty": "medium"
  },
  "logisticsAnalyzer": {
    "reveals": "delivery-timing-anomaly",
    "puzzleType": "route-optimization"
  }
}
```

### Clue Enhancement Types
- **chainScanner**: Supply chain vulnerability detection
- **logisticsAnalyzer**: Delivery timing analysis
- **jurisdictionPuzzle**: Customs authority conflict resolution
- **supplyNetwork**: Logistics route mapping
- **paradoxEvent**: Supply chain contradiction resolution

## Enhanced Method System

### Method Augmentations (Supply Chain Theme)
Methods now include logistics investigation data:

```json
{
  "id": "M1",
  "name": "Manifest Alteration",
  "signatures": ["digital-tampering", "routing-diversion"],
  "logisticsAnalysis": {
    "anomalyType": "delivery-disruption",
    "offsetMinutes": 11,
    "severity": "medium"
  }
}
```

### Method Enhancement Types
- **logisticsAnalysis**: Supply chain timing and routing analysis
- **chainScan**: Supply chain vulnerability signatures
- **supplyNetwork**: Logistics route relationship mapping

## Progression & Difficulty

### Supply Chain Complexity Escalation
- **Episodes 1-4:** Basic logistics investigation tools
- **Episodes 5-8:** Intermediate customs jurisdictional puzzles
- **Episodes 9-12:** Advanced supply network mapping

### Network Building
- Cases connect progressively through supply chain networks
- Medium paradox risk increases investigation stakes
- Creates foundation for Season 3's recovery networks

## Unity Implementation Notes

### UI Requirements (Season 2)
- **Supply Chain Map:** Global logistics route visualization
- **Logistics Analysis Interface:** Delivery timing and route optimization tools
- **Customs Jurisdiction Panel:** Authority conflict resolution interface
- **Chain Scan Display:** Vulnerability detection visualizations

### Puzzle Mechanics (Season 2)
- **Chain Scan Mini-Game:** Supply chain weakness pattern recognition
- **Route Optimization:** Logistics pathfinding and timing puzzles
- **Customs Resolution:** International shipping authority logic
- **Network Pathfinding:** Supply chain connection tracing

### Audio/Visual Feedback
- Logistics and shipping sound effects
- Supply chain route animations
- Customs clearance confirmations
- Medium-intensity paradox distortion effects

## Testing Guidelines

### Supply Chain Balance
- Test logistics flow maintains investigation momentum
- Verify supply chain theme integration feels authentic
- Ensure progressive difficulty supports player progression

### Mechanic Integration
- Confirm tools enhance supply chain investigation narrative
- Test multi-modal clue interactions are engaging
- Validate supply network connections provide strategic depth

### Performance Impact
- Monitor frame rate during supply network operations
- Test memory usage with enhanced logistics data
- Ensure mobile performance with investigation complexity

## Relationship to Other Seasons
Season 2 mechanics bridge Seasons 1 and 3:

- **Season 1 Corporate** → **Season 2 Supply Chain** → **Season 3 Recovery**
- **Data Scanner** → **Chain Scanner** → **Recovery Scanner**
- **Log Analyzer** → **Logistics Analyzer** → **Timing Analyzer**
- **Network Tracer** → **Supply Network** → **Paradox Mapper**
- **Corporate Jurisdiction** → **Customs Jurisdiction** → **Recovery Jurisdiction**

## Future Expansion
These Season 2 enhancements establish a foundation for:
- Advanced supply chain manipulation mechanics
- International logistics investigation complexity
- Cross-border jurisdictional puzzle systems
- Global supply network visualization tools</content>
<parameter name="filePath">/Users/gripandripphdd/Crimson-Compass/docs/Season2_EnhancedMechanics.md